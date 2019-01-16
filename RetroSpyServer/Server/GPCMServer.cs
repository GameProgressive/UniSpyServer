using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using GameSpyLib.Database;
using GameSpyLib.Network;
using GameSpyLib.Server;
using GameSpyLib.Logging;

namespace RetroSpyServer.Server
{
    /// <summary>
    /// This server emulates the Gamespy Client Manager Server on port 29900.
    /// This class is responsible for managing the login process.
    /// </summary>
    public class GPCMServer : PresenceServer
    {
        /// <summary>
        /// Indicates the timeout of when a connecting client will be disconnected
        /// </summary>
        public const int Timeout = 20000;

        /// <summary>
        /// A connection counter, used to create unique connection id's
        /// </summary>
        private long ConnectionCounter = 0;

        /// <summary>
        /// List of processing connections (id => Client Obj)
        /// </summary>
        private static ConcurrentDictionary<long, GPCMClient> Processing = new ConcurrentDictionary<long, GPCMClient>();

        /// <summary>
        /// List of sucessfully logged in clients (Pid => Client Obj)
        /// </summary>
        private static ConcurrentDictionary<long, GPCMClient> Clients = new ConcurrentDictionary<long, GPCMClient>();

        /// <summary>
        /// A Queue of player status updates we will make on the database in a batch update.
        /// </summary>
        public static ConcurrentQueue<PlayerStatusUpdate> PlayerStatusQueue { get; private set; } = new ConcurrentQueue<PlayerStatusUpdate>();

        /// <summary>
        /// Returns the number of players online
        /// </summary>
        /// <returns></returns>
        public int NumPlayersOnline
        {
            get { return Clients.Count; }
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

        /// <summary>
        /// Creates a new instance of <see cref="GPCMServer"/>
        /// </summary>
        public GPCMServer(DatabaseDriver databaseDriver) : base(databaseDriver)
        {
            GPCMClient.OnDisconnect += GpcmClient_OnDisconnect;
            GPCMClient.OnSuccessfulLogin += GpcmClient_OnSuccessfulLogin;

            // Setup timer. Every 15 seconds should be sufficient
            if (PollTimer == null || !PollTimer.Enabled)
            {
                PollTimer = new System.Timers.Timer(15000);
                PollTimer.Elapsed += (s, e) =>
                {
                    // Send keep alive to all connected clients
                    if (Clients.Count > 0)
                        Parallel.ForEach(Clients.Values, client => client.SendKeepAlive());

                    // Disconnect hanging connections
                    if (Processing.Count > 0)
                        Parallel.ForEach(Processing.Values, client => CheckTimeout(client));
                };
                PollTimer.Start();
            }

            // Setup timer. Every 5 seconds should be sufficient
            if (StatusTimer == null || !StatusTimer.Enabled)
            {
                StatusTimer = new System.Timers.Timer(5000);
                StatusTimer.Elapsed += (s, e) =>
                {
                    // Return if we are empty
                    if (PlayerStatusQueue.IsEmpty) return;

                    // Open database connection
                    using (var transaction = databaseDriver.BeginTransaction())
                    {
                        try

                        {
                            long timestamp = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
                            PlayerStatusUpdate result;
                            while (PlayerStatusQueue.TryDequeue(out result))
                            {
                                // Skip if this player never finished logging in
                                if (!result.Client.CompletedLoginProcess)
                                    continue;

                                // Only update record under these two status'
                                if (result.Status == LoginStatus.Completed && result.Status == LoginStatus.Disconnected)
                                {
                                    // Update player record
                                    databaseDriver.Execute(
                                        "UPDATE profiles SET status=@P3 lastip=@P0, lastonline=@P1 WHERE profileid=@P2",
                                        result.Client.RemoteEndPoint.Address,
                                        timestamp,
                                        result.Client.PlayerId,
                                        result.Status == LoginStatus.Completed ? 1 : 0
                                    );

                                    if (result.Status == LoginStatus.Completed)
                                        databaseDriver.Execute("UPDATE profiles SET sesskey=@P0 WHERE profileid=@P1", result.Client.SessionKey, result.Client.PlayerId);
                                    else
                                        databaseDriver.Execute("UDPATE profiles SET sesskey=null WHERE profileid=@P0", result.Client.PlayerId);
                                }
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            LogWriter.Log.WriteException(ex);
                            transaction.Rollback();
                        }
                    }
                };
                StatusTimer.Start();
            }

            // Set connection handling
            ConnectionEnforceMode = EnforceMode.DuringPrepare;

            // TODO: Change this
            //FullErrorMessage = Config.GetValue("Settings", "LoginServerFullMessage").Replace("\"", "");
            FullErrorMessage = "";
        }

        ~GPCMServer()
        {
            if (!Exiting)
                Shutdown();
        }

        /// <summary>
        /// Shutsdown the ClientManager server and socket
        /// </summary>
        public void Shutdown()
        {
            // Stop accepting new connections
            IgnoreNewConnections = true;
            Exiting = true;

            // Discard the poll timer
            PollTimer.Stop();
            PollTimer.Dispose();
            StatusTimer.Stop();
            StatusTimer.Dispose();

            // Disconnected all connected clients
            Console.WriteLine("Disconnecting all users...");
            Parallel.ForEach(Clients.Values, client => client.Disconnect(DisconnectReason.ForcedServerShutdown));
            Parallel.ForEach(Processing.Values, client => client.Disconnect(DisconnectReason.ForcedServerShutdown));

            // Update the database
            try
            {
                // Set everyone's online session to 0
                 databaseDriver.Execute("UPDATE profiles SET status=0, sesskey = NULL");
            }
            catch (Exception e)
            {
                LogWriter.Log.WriteException(e);
            }

            // Update Connected Clients in the Database
            Clients.Clear();

            // Tell the base to dispose all free objects
            Dispose();
        }

        /// <summary>
        /// Checks the timeout on a client connection. This method is used to detect hanging connections, and
        /// forcefully disconnects them.
        /// </summary>
        /// <param name="client"></param>
        protected void CheckTimeout(GPCMClient client)
        {
            // Setup vars
            DateTime expireTime = client.Created.AddSeconds(Timeout);
            GPCMClient oldC;

            // Remove all processing connections that are hanging
            if (client.LoginStatus != LoginStatus.Completed && expireTime <= DateTime.Now)
            {
                try
                {
                    client.Disconnect(DisconnectReason.LoginTimedOut);
                    Processing.TryRemove(client.ConnectionId, out oldC);
                }
                catch (Exception e)
                {
                    LogWriter.Log.WriteException(e);
                }
            }
            //else if (client.Status == LoginStatus.Completed)
            //{
            //Processing.TryRemove(client.ConnectionId, out oldC);
            //}
        }

        /// <summary>
        /// Returns whether the specified player is currently connected
        /// </summary>
        /// <param name="playerId">The players ID</param>
        /// <returns></returns>
        public bool IsConnected(int playerId)
        {
            return Clients.ContainsKey(playerId);
        }

        /// <summary>
        /// Forces the logout of a connected client
        /// </summary>
        /// <param name="playerId">The players ID</param>
        /// <returns>Returns whether the client was connected, and disconnect was called</returns>
        public bool ForceLogout(int playerId)
        {
            if (Clients.ContainsKey(playerId))
            {
                Clients[playerId].Disconnect(DisconnectReason.ForcedLogout);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Callback for when a connection had disconnected
        /// </summary>
        /// <param name="client">The client object whom is disconnecting</param>
        private void GpcmClient_OnDisconnect(GPCMClient client)
        {
            // If we are exiting, don't do anything here.
            if (Exiting) return;

            // Remove client, and call OnUpdate Event
            try
            {
                // Remove client from online list
                if (Clients.TryRemove(client.PlayerId, out client) && !client.Disposed)
                    client.Dispose();

                // Add player to database queue
                var status = new PlayerStatusUpdate(client, LoginStatus.Disconnected);
                PlayerStatusQueue.Enqueue(status);
            }
            catch (Exception e)
            {
                LogWriter.Log.Write("An Error occured at [GpcmServer._OnDisconnect] : Generating Exception Log {0}", LogLevel.Error, e.ToString());
            }
        }

        /// <summary>
        /// Callback for a successful login
        /// </summary>
        /// <param name="sender">The GpcmClient that is logged in</param>
        private void GpcmClient_OnSuccessfulLogin(object sender)
        {
            // Wrap this in a try/catch
            try
            {
                GPCMClient oldC;
                GPCMClient client = sender as GPCMClient;

                // Remove connection from processing
                Processing.TryRemove(client.ConnectionId, out oldC);

                // Check to see if the client is already logged in, if so disconnect the old user
                if (Clients.TryRemove(client.PlayerId, out oldC))
                {
                    oldC.Disconnect(DisconnectReason.NewLoginDetected);
                    LogWriter.Log.Write("Login Clash:   {0} - {1} - {2}", LogLevel.Information, client.PlayerNick, client.PlayerId, client.RemoteEndPoint);
                }

                // Add current client to the dictionary
                if (!Clients.TryAdd(client.PlayerId, client))
                    LogWriter.Log.Write("ERROR: [GpcmServer._OnSuccessfulLogin] Unable to add client to HashSet.", LogLevel.Error);

                // Add player to database queue
                var status = new PlayerStatusUpdate(client, LoginStatus.Completed);
                PlayerStatusQueue.Enqueue(status);
            }
            catch (Exception E)
            {
                LogWriter.Log.WriteException(E);
            }
        }

        protected override void OnException(Exception e) => LogWriter.Log.Write(e.Message, LogLevel.Error);

        protected override void ProcessAccept(TCPStream Stream)
        {
            // Get our connection id
            long connId = Interlocked.Increment(ref ConnectionCounter);
            GPCMClient client;

            try
            {
                // Create a new GpcmClient, passing the IO object for the TcpClientStream
                client = new GPCMClient(Stream, connId, databaseDriver);
                Processing.TryAdd(connId, client);

                // Begin the asynchronous login process
                client.SendServerChallenge(1);
            }
            catch (Exception e)
            {
                // Log the error
                LogWriter.Log.WriteException(e);

                // Remove pending connection
                Processing.TryRemove(connId, out client);

                // Release this stream so it can be used again
                Release(Stream);
            }
        }
    }
}
