using GameSpyLib.Common.BaseClass;
using GameSpyLib.Logging;
using GameSpyLib.MiscMethod;
using StatsAndTracking.Entity.Structure;
using StatsAndTracking.Handler.CommandHandler.Auth;
using StatsAndTracking.Handler.CommandHandler.AuthP;
using StatsAndTracking.Handler.CommandHandler.GetPD;
using StatsAndTracking.Handler.CommandHandler.GetPID;
using StatsAndTracking.Handler.CommandHandler.NewGame;
using StatsAndTracking.Handler.CommandHandler.SetPD;
using StatsAndTracking.Handler.CommandHandler.UpdGame;
using System;
using System.Linq;

namespace StatsAndTracking.Handler.CommandSwitcher
{
    public class GStatsCommandSwitcher : CommandSwitcherBase
    {
        public void Switch(GStatsSession session, string rawRequest)
        {
            try
            {
                if (rawRequest[0] != '\\')
                {
                    return;
                }
                string[] requestFraction = rawRequest.TrimStart('\\').Split('\\');
                var request = GameSpyUtils.ConvertRequestToKeyValue(requestFraction);

                switch (request.Keys.First())
                {
                    case GStatsRequestName.Auth:
                        new AuthHandler(session, request).Handle();
                        break;
                    case GStatsRequestName.AuthP:
                        new AuthPHandler(session, request).Handle();
                        break;
                    case GStatsRequestName.GetPid:
                        new GetPIDHandler(session, request).Handle();
                        break;
                    case GStatsRequestName.GetPD:
                        new GetPDHandler(session, request).Handle();
                        break;
                    case GStatsRequestName.SetPD:
                        new SetPDHandler(session, request).Handle();
                        break;
                    case GStatsRequestName.UpdGame:
                        new UpdGameHandler(session, request).Handle();
                        break;
                    case GStatsRequestName.NewGame:
                        new NewGameHandler(session, request).Handle();
                        break;
                    default:
                        LogWriter.UnknownDataRecieved(rawRequest);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.ToLog(Serilog.Events.LogEventLevel.Error, e.ToString());
            }
        }
    }
}
