using UniSpyLib.Logging;
using UniSpyLib.MiscMethod;
using Serilog.Events;
using StatsTracking.Entity.Structure;
using StatsTracking.Handler.CommandHandler.Auth;
using StatsTracking.Handler.CommandHandler.AuthP;
using StatsTracking.Handler.CommandHandler.GetPD;
using StatsTracking.Handler.CommandHandler.GetPID;
using StatsTracking.Handler.CommandHandler.NewGame;
using StatsTracking.Handler.CommandHandler.SetPD;
using StatsTracking.Handler.CommandHandler.UpdGame;
using System;
using System.Linq;

namespace StatsTracking.Handler.CommandSwitcher
{
    public class GStatsCommandSwitcher
    {
        public static void Switch(STSession session, string rawRequest)
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
                    case GStatsRequestName.AuthenticateUser:
                        new AuthHandler(session, request).Handle();
                        break;
                    case GStatsRequestName.AuthenticatePlayer:
                        new AuthPHandler(session, request).Handle();
                        break;
                    case GStatsRequestName.GetProfileID:
                        new GetPIDHandler(session, request).Handle();
                        break;
                    case GStatsRequestName.GetPlayerData:
                        new GetPDHandler(session, request).Handle();
                        break;
                    case GStatsRequestName.SetPlayerData:
                        new SetPDHandler(session, request).Handle();
                        break;
                    case GStatsRequestName.UpdateGameData:
                        new UpdGameHandler(session, request).Handle();
                        break;
                    case GStatsRequestName.CreateNewGamePlayerData:
                        new NewGameHandler(session, request).Handle();
                        break;
                    default:
                        LogWriter.UnknownDataRecieved(rawRequest);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.ToLog(LogEventLevel.Error, e.ToString());
            }
        }
    }
}
