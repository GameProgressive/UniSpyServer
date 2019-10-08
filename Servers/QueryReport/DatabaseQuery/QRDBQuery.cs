using GameSpyLib.Database;
using QueryReport.DedicatedServerData;
using System;

namespace QueryReport
{
    public class QRDBQuery
    {
        /// <summary>
        /// Sets a server's online status in the database
        /// </summary>
        /// <param name="server"></param>
        public void UpdateServerOffline(GameServerData server)
        {
            // Check if server exists in database
            if (server.DatabaseId > 0)
            {
                // Update
                string query = "UPDATE server SET online=0, lastseen=@P0 WHERE id=@P1";
                QRServer.DB.Execute(query, new DateTimeOffset(server.LastRefreshed).ToUnixTimeSeconds(), server.DatabaseId);
            }
        }

        internal bool FetchPlasmaServer(object address, object port)
        {
            throw new NotImplementedException();
        }
    }
}
