using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Structure.Misc;
using GameStatus.Handler.CmdHandler;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace GameStatus.Handler.CmdSwitcher
{
    internal sealed class GSCmdHandlerSerializer : UniSpyCmdHandlerFactoryBase
    {
        private new GSRequestBase _request => (GSRequestBase)base._request;
        public GSCmdHandlerSerializer(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        public override IUniSpyHandler Serialize()
        {
            switch (_request.CommandName)
            {
                case GSRequestName.AuthenticateUser:
                    return new AuthHandler(_session, _request);
                case GSRequestName.AuthenticatePlayer:
                    return new AuthPHandler(_session, _request);
                case GSRequestName.GetProfileID:
                    return new GetPIDHandler(_session, _request);
                case GSRequestName.GetPlayerData:
                    return new GetPDHandler(_session, _request);
                case GSRequestName.SetPlayerData:
                    return new SetPDHandler(_session, _request);
                case GSRequestName.UpdateGameData:
                    return new UpdGameHandler(_session, _request);
                case GSRequestName.CreateNewGamePlayerData:
                    return new NewGameHandler(_session, _request);
                default:
                    LogWriter.UnknownDataRecieved(_request.RawRequest);
                    return null;
            }
        }
    }
}
