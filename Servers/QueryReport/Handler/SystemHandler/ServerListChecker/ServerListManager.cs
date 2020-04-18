using GameSpyLib.Common.BaseClass;

namespace QueryReport.Handler.CommandHandler.ServerList
{
    public class ServerListManager : ExpireManagerBase
    {
        public ServerListManager()
        {
        }

        protected override void CheckExpire()
        {
            base.CheckExpire();

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
