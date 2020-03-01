using QueryReport.Entity.Structure;
using QueryReport.Server;
using System;
using System.Collections.Generic;
namespace QueryReport.Handler.CommandHandler.ServerList
{
    public class ServerListChecker
    {

        private System.Timers.Timer _checkTimer = new System.Timers.Timer { Enabled = true, Interval = 10000, AutoReset = true };//10000
        /// <summary>
        /// Executed every 5 seconds or so... Removes all servers that haven't
        /// reported in awhile
        /// </summary>
        public void StartCheck(QRServer server)
        {
            _checkTimer.Start();
            _checkTimer.Elapsed += (s, e) => CheckClientTimeOut(server);
        }
        private void CheckClientTimeOut(QRServer server)
        {
            server.ToLog("Check timeout excuted!");

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
