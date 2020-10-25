using GameSpyLib.Logging;
using GameSpyLib.MiscMethod;
using Serilog.Events;
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
    public class GStatsCommandSwitcher
    {
        public static void Switch(GStatsSession session, string rawRequest)
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
