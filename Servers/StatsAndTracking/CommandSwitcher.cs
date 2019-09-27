using GameSpyLib.Logging;
using StatsAndTracking.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StatsAndTracking
{
    public class CommandSwitcher
    {
        public static void Switch(GStatsClient client, Dictionary<string, string> recv)
        {
            string command = recv.Keys.First();
            try
            {
                switch (command)
                {
                    case "auth":
                        AuthHandler.SendSessionKey(client, recv);
                        break;
                    case "authp":
                        AuthPHandler.AuthPlayer(client, recv);
                    default:
                        LogWriter.Log.Write("[GSTAS] received unknown data " + command, LogLevel.Debug);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.Log.WriteException(e);
            }
        }
    }
}
