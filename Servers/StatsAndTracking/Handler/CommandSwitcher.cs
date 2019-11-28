using GameSpyLib.Logging;
using StatsAndTracking.Handler.Auth;
using StatsAndTracking.Handler.AuthP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatsAndTracking.Handler
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
