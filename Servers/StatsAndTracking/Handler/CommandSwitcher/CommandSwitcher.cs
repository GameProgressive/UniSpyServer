using GameSpyLib.Logging;
using StatsAndTracking.Handler.CommandHandler.Auth;
using StatsAndTracking.Handler.CommandHandler.AuthP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatsAndTracking.Handler.CommandSwitcher
{
    public class CommandSwitcher
    {
        public static void Switch(GStatsSession session, Dictionary<string, string> recv)
        {
            string command = recv.Keys.First();
            try
            {
                switch (command)
                {
                    case "auth":
                        AuthHandler auth = new AuthHandler(session, recv);
                        break;
                    case "authp":
                        AuthPHandler authp = new AuthPHandler(session, recv);
                        break;
                    case "getpid":
                        break;
                    case "getpd"://get player data
                        break;
                    case "setpd":
                        break;
                    case "updgame":
                        break;
                    case "newgame":
                        break;
                    default:
                        session.UnknownDataReceived(recv);
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
