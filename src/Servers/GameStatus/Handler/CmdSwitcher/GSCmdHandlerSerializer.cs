using UniSpyLib.Logging;
using GameStatus.Entity.Structure;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Abstraction.BaseClass;
using GameStatus.Abstraction.BaseClass;
using GameStatus.Handler.CmdHandler;

namespace GameStatus.Handler.CmdSwitcher
{
    internal sealed class GSCmdHandlerSerializer : UniSpyCmdHandlerSerializerBase
    {
        private new GSRequestBase _request => (GSRequestBase)base._request;
        public GSCmdHandlerSerializer(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        public override IUniSpyHandler Serialize()
        {
            switch (_request.CommandName)
            {
                case GStatsRequestName.AuthenticateUser:
                    return new AuthHandler(_session, _request);
                case GStatsRequestName.AuthenticatePlayer:
                    return new AuthPHandler(_session, _request);
                case GStatsRequestName.GetProfileID:
                    return new GetPIDHandler(_session, _request);
                case GStatsRequestName.GetPlayerData:
                    return new GetPDHandler(_session, _request);
                case GStatsRequestName.SetPlayerData:
                    return new SetPDHandler(_session, _request);
                case GStatsRequestName.UpdateGameData:
                    return new UpdGameHandler(_session, _request);
                case GStatsRequestName.CreateNewGamePlayerData:
                    return new NewGameHandler(_session, _request);
                default:
                    LogWriter.UnknownDataRecieved(_request.RawRequest);
                    return null;
            }
        }
    }
}
