using GameSpyLib.Common.BaseClass;
using GameSpyLib.Logging;
using GameSpyLib.MiscMethod;
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
    public class StatsCommandSwitcher : CommandSwitcherBase
    {
        public void Switch(GStatsSession session, string message)
        {
            try
            {
                if (message[0] != '\\')
                {
                    return;
                }
                string[] recieved = message.TrimStart('\\').Split('\\');
                Dictionary<string, string> recv = GameSpyUtils.ConvertRequestToKeyValue(recieved);

                switch (recv.Keys.First())
                {
                    case "auth":
                        new AuthHandler().Handle(session, recv);
                        break;
                    case "authp":
                        new AuthPHandler().Handle(session, recv);
                        break;
                    case "getpid":
                        new GetPidHandler().Handle(session, recv);
                        break;
                    case "getpd"://get player data
                        new GetPDHandler().Handle(session, recv);
                        break;
                    case "setpd":
                        new SetPDHandler().Handle(session, recv);
                        break;
                    case "updgame":
                        new UpdGameHandler().Handle(session, recv);
                        break;
                    case "newgame":
                        new NewGameHandler().Handle(session, recv);
                        break;
                    default:
                        LogWriter.UnknownDataRecieved(message);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.ToLog(e);
            }
        }
    }
}
