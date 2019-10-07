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
                        AuthpHandler.AuthPlayer(session, recv);
                        break;
                    default:
                        session.Server.ToLog("received unknown data " + command);
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
