using GameSpyLib.Logging;
using GameSpyLib.Network;
using RetroSpyServer.Servers.MasterServer.GameServerInfo;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using GameSpyLib.Database;
using RetroSpyServer.DBQueries;

namespace RetroSpyServer.Servers.MasterServer
{
    public class MasterServer : UdpServer
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
        public static ConcurrentDictionary<string, GameServer> Servers = new ConcurrentDictionary<string, GameServer>();

        /// <summary>
        /// A timer that is used to Poll all the servers, and remove inactive servers from the server list
        /// </summary>
        private static Timer PollTimer;

        /// <summary>
        /// The Time for servers are to remain in the serverlist since the last ping.
        /// Once this value is surpassed, server is presumed offline and is removed
        /// </summary>
        public static int ServerTTL { get; protected set; }

        public MasterServer(DatabaseDriver driver,IPEndPoint bindTo, int MaxConnection) : base(bindTo, MaxConnection)
        {
            MasterDBQuery.DBQuery = new DBQueries.GPCMDBQuery(driver);
            //The Time for servers to remain in the serverlist since the last ping in seconds.
            //This value must be greater than 20 seconds, as that is the ping rate of the server
            //Suggested value is 30 seconds, this gives the server some time if the master server
            //is busy and cant refresh the server's TTL right away
            ServerTTL = 30;
            // Setup timer. Remove servers who havent ping'd since ServerTTL
            PollTimer = new Timer(5000);
            PollTimer.Elapsed += (s, e) => CheckServers();
            PollTimer.Start();
        }
        /// <summary>
        /// Callback method for when the UDP Master socket recieves a connection
        /// </summary>
        protected override void ProcessAccept(UdpPacket packet)
        {

        }


        protected override void OnException(Exception e) => LogWriter.Log.WriteException(e);


        /// <summary>
        /// Executed every 5 seconds or so... Removes all servers that haven't
        /// reported in awhile
        /// </summary>
        protected void CheckServers()
        {
            // Create a list of servers to update in the database
            List<GameServer> ServersToRemove = new List<GameServer>();
            var span = TimeSpan.FromSeconds(ServerTTL);

            // Remove servers that havent talked to us in awhile from the server list
            foreach (string key in Servers.Keys)
            {
                GameServer value;
                if (Servers.TryGetValue(key, out value))
                {
                    if (value.LastPing < DateTime.Now - span)
                    {                       
                        LogWriter.Log.Write("Removing Server for Expired Ping: " + key,LogLevel.Debug);
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
                    var transaction = MasterDBQuery.DBQuery.BeginTransaction();
                {
                    try
                    {
                        foreach (GameServer server in ServersToRemove)
                            Driver.UpdateServerOffline(server);

                        Transaction.Commit();
                    }
                    catch
                    {
                        Transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                Program.ErrorLog.Write("ERROR: [MasterDatabase.UpdateServerOffline] Unable to update servers status: " + e.Message);
            }
        }
       
        
    }
}
