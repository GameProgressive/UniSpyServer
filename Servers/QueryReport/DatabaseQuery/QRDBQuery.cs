using GameSpyLib.Database;
using QueryReport.GameServerInfo;
using System;

namespace QueryReport
{
    public class QRDBQuery : DBQueryBase
    {
        public QRDBQuery(DatabaseDriver dbdriver) : base(dbdriver)
        {
        }

        /// <summary>
        /// Sets a server's online status in the database
        /// </summary>
        /// <param name="server"></param>
        public void UpdateServerOffline(DedicatedServer server)
        {
            // Check if server exists in database
            if (server.DatabaseId > 0)
            {
                // Update
                string query = "UPDATE server SET online=0, lastseen=@P0 WHERE id=@P1";
                Execute(query, new DateTimeOffset(server.LastRefreshed).ToUnixTimeSeconds(), server.DatabaseId);
            }
        }
    }
}
