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
        public static void Switch(GstatsSession session, Dictionary<string, string> recv)
        {
            string command = recv.Keys.First();
            try
            {
                switch (command)
                {
                    case "auth":
                        AuthHandler.SendSessionKey(session, recv);
                        break;
                    case "authp":
                        AuthPHandler.AuthPlayer(session, recv);
                        break;
                    case "getpd"://get player data
                        break;
                    default:
                        session.UnknownDataRecived(recv);
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
