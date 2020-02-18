using QueryReport.Entity.Structure;
using QueryReport.Server;
using System;
using System.Collections.Generic;
namespace QueryReport.Handler.CommandHandler.ServerList
{
    public class ServerListHandler
    {
        /// <summary>
        /// Executed every 5 seconds or so... Removes all servers that haven't
        /// reported in awhile
        /// </summary>
        public static void CheckServers()
        {
            // Create a list of servers to update in the database
            List<GameServerData> ServersToRemove = new List<GameServerData>();

            var timeSpan = TimeSpan.FromSeconds(QRServer.ServerTTL);

            // Remove servers that havent talked to us in awhile from the server list
            //foreach (string key in QRServer.ServersList.Keys)
            //{
            //    GameServerData gameServer;
            //    if (QRServer.ServersList.TryGetValue(key, out gameServer))
            //    {
            //        if (gameServer.LastPing < DateTime.Now - timeSpan)
            //        {
            //            LogWriter.Log.Write("Removing Server for Expired Ping: " + key, LogLevel.Info);
            //            if (QRServer.ServersList.TryRemove(key, out gameServer))
            //                ServersToRemove.Add(gameServer);
            //            else
            //            {
            //                string error = string.Format(@" Unable to remove server from server list: " + key);
            //                LogWriter.Log.Write(error, LogLevel.Error);
            //            }
            //        }
            //    }
            //}
        }
    }
}
