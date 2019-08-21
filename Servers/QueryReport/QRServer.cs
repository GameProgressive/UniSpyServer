using GameSpyLib.Database;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using QueryReport.GameServerInfo;
using QueryReport.Handler;
using QueryReport.Structures;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Timers;

namespace QueryReport
{
    public class QRServer : UdpServer
    {
        /// <summary>
        /// Max number of concurrent open and active connections.
        /// </summary>
        /// <remarks>
        ///   While fast, the BF2Available requests will shoot out 6-8 times
        ///   per client while starting up BF2, so i set this alittle higher then usual.
        ///   Servers also post their data here, and a lot of servers will keep the
        ///   connections rather high.

        /// <summary>
        /// A List of all servers that have sent data to this master server, and are active in the last 30 seconds or so
        /// </summary>
        public static ConcurrentDictionary<string, DedicatedServer> Servers = new ConcurrentDictionary<string, DedicatedServer>();

        /// <summary>
        /// A timer that is used to Poll all the servers, and remove inactive servers from the server list
        /// </summary>
        private static Timer PollTimer;

        /// <summary>
        /// The Time for servers are to remain in the serverlist since the last ping.
        /// Once this value is surpassed, server is presumed offline and is removed
        /// </summary>
        public static int ServerTTL { get; protected set; }

        public bool Replied = false;

        public QRServer(string serverName, DatabaseDriver driver, IPEndPoint bindTo, int MaxConnection) : base(serverName, bindTo, MaxConnection)
        {
            QRHandler.DBQuery = new QRDBQuery(driver);

            //The Time for servers to remain in the serverlist since the last ping in seconds.
            //This value must be greater than 20 seconds, as that is the ping rate of the server
            //Suggested value is 30 seconds, this gives the server some time if the master server
            //is busy and cant refresh the server's TTL right away
            ServerTTL = 30;

            // Setup timer. Remove servers who havent ping'd since ServerTTL
            PollTimer = new Timer(5000);
            PollTimer.Elapsed += (s, e) => CheckServers();
            PollTimer.Start();

            StartAcceptAsync();
        }
        /// <summary>
        /// Callback method for when the UDP Query Report socket recieves a connection
        /// </summary>
        protected override void ProcessAccept(UdpPacket packet)
        {
            IPEndPoint remote = (IPEndPoint)packet.AsyncEventArgs.RemoteEndPoint;

            // Need at least 5 bytes
            if (packet.BytesRecieved.Length < 5)
            {
                Release(packet.AsyncEventArgs);
                return;
            }

            // If we dont reply, we must manually release the EventArgs back to the pool
            Replied = false;
            try
            {
                switch (packet.BytesRecieved[0])
                {
                    // Note: BattleSpy make use of this despite not being used in both OpenSpy and the SDK.
                    // Perhaps it was present on an older version of GameSpy SDK
                    case QRGameServerRequest.Challenge:
                        ChallengeHandler.ServerChallengeResponse(this, packet);
                        break;
                    case QRClientRequest.Heartbeat: // HEARTBEAT
                        HeartBeatHandler.HeartbeatResponse(this, packet);
                        break;
                    case QRClientRequest.KeepAlive:
                        KeepAliveHandler.KeepAliveResponse(this, packet);
                        break;
                    case QRClientRequest.Avaliable:
                        AvaliableCheckHandler.BackendAvaliabilityResponse(this, packet);
                        break;
                    default:
                        LogWriter.Log.Write("[QR] [Recv] unknown data: ", LogLevel.Error);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.Log.WriteException(e);
            }
            finally
            {
                //if we replied QR data we release packet so that the EventArgs can be used on another connection
                if (Replied == true)
                    Release(packet.AsyncEventArgs);
            }

        }


        protected override void OnException(Exception e) => LogWriter.Log.WriteException(e);


        /// <summary>
        /// Executed every 5 seconds or so... Removes all servers that haven't
        /// reported in awhile
        /// </summary>
        protected void CheckServers()
        {
            // Create a list of servers to update in the database
            List<DedicatedServer> ServersToRemove = new List<DedicatedServer>();
            var span = TimeSpan.FromSeconds(ServerTTL);

            // Remove servers that havent talked to us in awhile from the server list
            foreach (string key in Servers.Keys)
            {
                DedicatedServer value;
                if (Servers.TryGetValue(key, out value))
                {
                    if (value.LastPing < DateTime.Now - span)
                    {
                        LogWriter.Log.Write("Removing Server for Expired Ping: " + key, LogLevel.Debug);
                        if (Servers.TryRemove(key, out value))
                            ServersToRemove.Add(value);
                        else
                            LogWriter.Log.Write("[MasterServer.CheckServers] Unable to remove server from server list: " + key, LogLevel.Error);

                    }
                }
            }

            // If we have no servers to update, return
            if (ServersToRemove.Count == 0) return;

            // Update servers in database
            try
            {
                // Wrap this all in a database transaction, as this will speed
                // things up alot if there are alot of rows to update
                //using (DatabaseDriver Driver = new DatabaseDriver())
                //using (DbTransaction Transaction = Driver.BeginTransaction())
                var transaction = QRHandler.DBQuery.BeginTransaction();

                {
                    try
                    {
                        foreach (DedicatedServer server in ServersToRemove)
                            QRHandler.UpdateServerOffline(server);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                LogWriter.Log.WriteException(e);
            }
        }
    }
}
