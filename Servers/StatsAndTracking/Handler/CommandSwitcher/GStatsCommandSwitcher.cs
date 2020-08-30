using System;
using System.Collections.Generic;
using System.Linq;
using GameSpyLib.Common.BaseClass;
using GameSpyLib.Logging;
using GameSpyLib.MiscMethod;
using StatsAndTracking.Handler.CommandHandler.Auth;
using StatsAndTracking.Handler.CommandHandler.AuthP;
using StatsAndTracking.Handler.CommandHandler.GetPD;
using StatsAndTracking.Handler.CommandHandler.GetPID;
using StatsAndTracking.Handler.CommandHandler.NewGame;
using StatsAndTracking.Handler.CommandHandler.SetPD;
using StatsAndTracking.Handler.CommandHandler.UpdGame;

namespace StatsAndTracking.Handler.CommandSwitcher
{
    public class GStatsCommandSwitcher : CommandSwitcherBase
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
                var recv = GameSpyUtils.ConvertRequestToKeyValue(recieved);

                switch (recv.Keys.First())
                {
                    case "auth"://authentication
                        new AuthHandler(session, recv).Handle();
                        break;
                    case "authp"://authenticate player
                        new AuthPHandler(session, recv).Handle();
                        break;
                    case "getpid"://get player from profileid
                        new GetPIDHandler(session, recv).Handle();
                        break;
                    case "getpd"://get player data
                        new GetPDHandler(session, recv).Handle();
                        break;
                    case "setpd"://send player data
                        new SetPDHandler(session, recv).Handle();
                        break;
                    case "updgame"://update a profile data for a game
                        new UpdGameHandler(session, recv).Handle();
                        break;
                    case "newgame"://create new player data for a game
                        new NewGameHandler(session, recv).Handle();
                        break;
                    default:
                        LogWriter.UnknownDataRecieved(message);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.ToLog(Serilog.Events.LogEventLevel.Error,e.ToString());
            }
        }
    }
}
