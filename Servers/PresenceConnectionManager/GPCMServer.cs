using GameSpyLib.Database;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using NetCoreServer;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler.General.KeepAlive;
using PresenceConnectionManager.Handler.General.Login.Query;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading.Tasks;

//GPCM represents GameSpy Connection Manager
namespace PresenceConnectionManager
{
    /// <summary>
    /// This server emulates the Gamespy Client Manager Server on port 29900.
    /// This class is responsible for managing the login process.
    /// </summary>
    public class GPCMServer : TemplateTcpServer
    {
        /// <summary>
        /// Indicates the timeout of when a connecting client will be disconnected
        /// </summary>
        public const int ExpireTime = 20000;

        /// <summary>
        /// List of processing connections (id => Client Obj)
        /// </summary>
        private static ConcurrentDictionary<Guid, GPCMSession> InLoginSession = new ConcurrentDictionary<Guid, GPCMSession>();

        /// <summary>
        /// List of sucessfully logged in clients (Pid => Client Obj)
        /// </summary>
        private static ConcurrentDictionary<Guid, GPCMSession> LoggedInSession = new ConcurrentDictionary<Guid, GPCMSession>();

        /// <summary>
        /// A Queue of player status updates we will make on the database in a batch update.
        /// </summary>
        public static ConcurrentQueue<GPCMSession> PlayerStatusQueue { get; private set; } = new ConcurrentQueue<GPCMSession>();

        /// <summary>
        /// Returns the number of players online
        /// </summary>
        /// <returns></returns>
        public int NumPlayersOnline
        {
            get { return LoggedInSession.Count; }
        }

        /// <summary>
        /// A timer that is used to Poll all connections, and removes dropped connections
        /// </summary>
        public static System.Timers.Timer PollTimer { get; protected set; }

        /// <summary>
        /// A timer that is used to batch all PlayerStatusUpdates into the database
        /// </summary>
        public static System.Timers.Timer StatusTimer { get; protected set; }

        /// <summary>
        /// Indicates whether we are closing the server down
        /// </summary>
        public bool Exiting { get; private set; } = false;


        public static DatabaseDriver DB;
        /// <summary>
        /// Creates a new instance of <see cref="GPCMClient"/>
        /// </summary>
        public GPCMServer(string serverName, DatabaseDriver databaseDriver, IPAddress address, int port) : base(serverName, address, port)
        {

            //GPCMHandler.DBQuery = new GPCMDBQuery(driver);

            DB = databaseDriver;
            KeepAliveManagement();
            PlayerStatusUpdate();
            // Setup timer. Every 15 seconds should be sufficient
            // Set connection handling           
        }
        protected override TcpSession CreateSession() { return new GPCMSession(this); }
        private void PlayerStatusUpdate()
        {
            //the status of a player may change time by time we need to update that, so the online status will update on others
            // Setup timer. Every 5 seconds should be sufficient
            if (StatusTimer == null || !StatusTimer.Enabled)
            {
                StatusTimer = new System.Timers.Timer(5000);
                StatusTimer.Elapsed += (s, e) =>
                {
                    // Return if we are empty
                    if (PlayerStatusQueue.IsEmpty) return;
                    //var transaction =DB.BeginTransaction();
                    try
                    {
                        long timestamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
                        GPCMSession session;
                        while (PlayerStatusQueue.TryDequeue(out session))
                        {
                            // Skip if this player never finished logging in
                            if (session == null)
                                continue;
                            if (!session.CompletedLoginProcess)
                                continue;
                            LoginQuery.UpdateStatus(timestamp, session.Remote.ToString(), session.PlayerInfo.Profileid, (uint)session.PlayerInfo.PlayerStatus);
                        }
                        //transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        LogWriter.Log.WriteException(ex);
                        // transaction.Rollback();
                    }
                };
                StatusTimer.Start();
            }
        }
        private void KeepAliveManagement()
        {
            // Setup timer. Every 15 seconds should be sufficient
            if (PollTimer == null || !PollTimer.Enabled)
            {
                PollTimer = new System.Timers.Timer(15000);
                PollTimer.Elapsed += (s, e) =>
                {
                    // Send keep alive to all connected clients
                    if (LoggedInSession.Count > 0)
                    {
                        Parallel.ForEach(LoggedInSession.Values, client => KAHandler.SendKeepAlive(client));
                    }
                    // DisconnectByReason hanging connections
                    if (InLoginSession.Count > 0)
                    {
                        Parallel.ForEach(InLoginSession.Values, client => CheckTimeout(client));
                    }
                };
                PollTimer.Start();
            }
        }
        private bool _disposed;

        protected override void Dispose(bool disposingManagedResources)
        {
            if (_disposed) return;
            _disposed = true;
            if (disposingManagedResources)
            {

            }
            PollTimer?.Stop();
            PollTimer?.Dispose();
            StatusTimer?.Stop();
            StatusTimer?.Dispose();
            // Disconnected all connected clients
            Console.WriteLine("Disconnecting all users...");
            Parallel.ForEach(LoggedInSession.Values, client => client.DisconnectByReason(DisconnectReason.ForcedServerShutdown));
            Parallel.ForEach(InLoginSession.Values, client => client.DisconnectByReason(DisconnectReason.ForcedServerShutdown));
            LoginQuery.ResetAllStatusAndSessionKey();
            LoggedInSession.Clear();
            DB.Dispose();
            base.Dispose(disposingManagedResources);
        }


        /// <summary>
        /// Checks the timeout on a client connection. This method is used to detect hanging connections, and
        /// forcefully disconnects them.
        /// </summary>
        /// <param name="client"></param>
        protected void CheckTimeout(GPCMSession session)
        {
            // Setup vars
            DateTime expireTime = session.Created.AddSeconds(ExpireTime);
            GPCMSession oldSession;
            // Remove all processing connections that are hanging
            if (session.PlayerInfo.LoginProcess != LoginStatus.Completed && expireTime <= DateTime.Now)
            {
                try
                {
                    session.Disconnect();
                }
                catch (Exception e)
                {
                    LogWriter.Log.WriteException(e);
                }
            }
            else if (session.PlayerInfo.LoginProcess == LoginStatus.Completed)
            {
                InLoginSession.TryRemove(session.Id, out oldSession);
            }
        }

        /// <summary>
        /// Returns whether the specified player is currently connected
        /// </summary>
        /// <param name="playerId">The players ID</param>
        /// <returns></returns>
        public bool IsConnected(Guid guid)
        {
            return LoggedInSession.ContainsKey(guid);
        }

        /// <summary>
        /// Forces the logout of a connected client
        /// </summary>
        /// <param name="playerId">The players ID</param>
        /// <returns>Returns whether the client was connected, and disconnect was called</returns>
        public bool ForceLogout(Guid guid)
        {
            if (LoggedInSession.ContainsKey(guid))
            {
                LoggedInSession[guid].DisconnectByReason(DisconnectReason.ForcedLogout);
                return true;
            }
            return false;
        }

    }


}
