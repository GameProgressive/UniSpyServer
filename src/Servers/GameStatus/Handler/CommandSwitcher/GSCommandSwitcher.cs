using UniSpyLib.Logging;
using UniSpyLib.MiscMethod;
using Serilog.Events;
using GameStatus.Entity.Structure;
using GameStatus.Handler.CommandHandler.Auth;
using GameStatus.Handler.CommandHandler.AuthP;
using GameStatus.Handler.CommandHandler.GetPD;
using GameStatus.Handler.CommandHandler.GetPID;
using GameStatus.Handler.CommandHandler.NewGame;
using GameStatus.Handler.CommandHandler.SetPD;
using GameStatus.Handler.CommandHandler.UpdGame;
using System;
using System.Linq;
using UniSpyLib.Abstraction.Interface;

namespace GameStatus.Handler.CommandSwitcher
{
    public class GSCommandSwitcher
    {
        public static void Switch(ISession session, string rawRequest)
        {
            try
            {
                if (rawRequest[0] != '\\')
                {
                    return;
                }
                string[] requestFraction = rawRequest.TrimStart('\\').Split('\\');
                var request = GameSpyUtils.ConvertToKeyValue(requestFraction);

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
