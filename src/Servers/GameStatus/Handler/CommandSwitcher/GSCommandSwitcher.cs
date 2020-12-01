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
using UniSpyLib.Abstraction.BaseClass;

namespace GameStatus.Handler.CommandSwitcher
{
    public class GSCommandSerializer : CommandHandlerSerializerBase
    {
        public GSCommandSerializer(ISession session, IRequest request) : base(session, request)
        {
        }

        public override void Serialize()
        {
            try
            {
                if (_rawRequest[0] != '\\')
                {
                    return;
                }
                string[] requestFraction = _rawRequest.TrimStart('\\').Split('\\');
                var request = GameSpyUtils.ConvertToKeyValue(requestFraction);

                switch (request.Keys.First())
                {
                    case GStatsRequestName.AuthenticateUser:
                        new AuthHandler(_session, request).Handle();
                        break;
                    case GStatsRequestName.AuthenticatePlayer:
                        new AuthPHandler(_session, request).Handle();
                        break;
                    case GStatsRequestName.GetProfileID:
                        new GetPIDHandler(_session, request).Handle();
                        break;
                    case GStatsRequestName.GetPlayerData:
                        new GetPDHandler(_session, request).Handle();
                        break;
                    case GStatsRequestName.SetPlayerData:
                        new SetPDHandler(_session, request).Handle();
                        break;
                    case GStatsRequestName.UpdateGameData:
                        new UpdGameHandler(_session, request).Handle();
                        break;
                    case GStatsRequestName.CreateNewGamePlayerData:
                        new NewGameHandler(_session, request).Handle();
                        break;
                    default:
                        LogWriter.UnknownDataRecieved(_rawRequest);
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
