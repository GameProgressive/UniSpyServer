using GameSpyLib.Logging;
using QueryReport.DedicatedServerData;
using System;
using System.Collections.Generic;

namespace QueryReport.Handler.ServerList
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
            foreach (string key in QRServer.ServersList.Keys)
            {
                GameServerData gameServer;
                if (QRServer.ServersList.TryGetValue(key, out gameServer))
                {
                    if (gameServer.LastPing < DateTime.Now - timeSpan)
                    {
                        LogWriter.Log.Write("Removing Server for Expired Ping: " + key, LogLevel.Info);
                        if (QRServer.ServersList.TryRemove(key, out gameServer))
                            ServersToRemove.Add(gameServer);
                        else
                        {
                            string error = string.Format(@" Unable to remove server from server list: " + key);
                            LogWriter.Log.Write(error, LogLevel.Error);
                        }
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
                using (var transaction = QRServer.DB.BeginTransaction())
                {
                    try
                    {
                        foreach (GameServerData server in ServersToRemove)
                            ServerListQuery.UpdateServerOffline(server);
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
