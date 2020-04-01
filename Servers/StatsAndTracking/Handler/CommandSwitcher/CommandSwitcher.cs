using StatsAndTracking.Handler.CommandHandler.Auth;
using StatsAndTracking.Handler.CommandHandler.AuthP;
using StatsAndTracking.Handler.CommandHandler.GetPD;
using StatsAndTracking.Handler.CommandHandler.GetPid;
using StatsAndTracking.Handler.CommandHandler.NewGame;
using StatsAndTracking.Handler.CommandHandler.SetPD;
using StatsAndTracking.Handler.CommandHandler.UpdGame;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StatsAndTracking.Handler.CommandSwitcher
{
    public class CommandSwitcher
    {
        public static void Switch(GStatsSession session, Dictionary<string, string> recv)
        {
            
            try
            {
                switch (recv.Keys.First())
                {
                    case "auth":
                        new AuthHandler(session, recv);
                        break;
                    case "authp":
                        new AuthPHandler(session, recv);
                        break;
                    case "getpid":
                        new GetPidHandler(session, recv);
                        break;
                    case "getpd"://get player data
                        new GetPDHandler(session, recv);
                        break;
                    case "setpd":
                        new SetPDHandler(session, recv);
                        break;
                    case "updgame":
                        new UpdGameHandler(session, recv);
                        break;
                    case "newgame":
                        new NewGameHandler(session, recv);
                        break;
                    default:
                        session.UnknownDataReceived(recv);
                        break;
                }
            }
            catch (Exception e)
            {
                session.ToLog(Serilog.Events.LogEventLevel.Error, e.ToString());
            }
        }
    }
}
