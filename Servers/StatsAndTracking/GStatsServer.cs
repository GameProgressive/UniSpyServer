using GameSpyLib.Database;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using NetCoreServer;
using System;
using System.Net;
using System.Net.Sockets;

namespace StatsAndTracking
{
    public class GStatsServer : TemplateTcpServer
    {
        public static DatabaseDriver DB;

        public bool Disposed = false;
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="databaseDriver">
        /// A connection to a database
        /// If the databaseDriver is null, then the server will attempt to create it's own connection
        /// otherwise it will use the specified connection
        /// </param>
        public GStatsServer(string serverName,DatabaseDriver databaseDriver, IPAddress address, int port) : base(serverName,address, port)
        {
            //GStatsHandler.DBQuery = new GSTATSDBQuery(databaseDriver);
            DB = databaseDriver;
            // Begin accepting connections
            Start();
        }

        ~GStatsServer()
        {
            Dispose(false);
        }

        protected override TcpSession CreateSession() { return new GstatsSession(this); }

        protected override void Dispose(bool disposingManagedResources)
        {
            if (disposingManagedResources)
            {

            }
            DB.Close();
            DB?.Dispose();
            DB = null;

            base.Dispose(disposingManagedResources);
        }

        public string SendServerChallenge()
        {
            //38byte
            string serverChallengeKey = GameSpyLib.Common.Random.GenerateRandomString(38, GameSpyLib.Common.Random.StringType.Alpha);
            //string sendingBuffer = string.Format(@"\challenge\{0}\final\", ServerChallengeKey);
            //sendingBuffer = xor(sendingBuffer);
            string sendingBuffer = string.Format(@"\challenge\{0}", serverChallengeKey);
            sendingBuffer = GameSpyLib.Extensions.Enctypex.XorEncoding(sendingBuffer, 1);
            return sendingBuffer;
        }
    }
}
