using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Contract;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Response;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.GameStatus.Handler.CmdHandler
{
    [HandlerContract("auth")]
    public sealed class AuthGameHandler : CmdHandlerBase
    {
        //UniSpyServer.UniSpyLib.Encryption.Crc16 _crc16 = new UniSpyServer.UniSpyLib.Encryption.Crc16(UniSpyServer.UniSpyLib.Encryption.Crc16Mode.Standard);
        private new AuthGameRequest _request => (AuthGameRequest)base._request;
        private new AuthResult _result{ get => (AuthResult)base._result; set => base._result = value; }
        public AuthGameHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new AuthResult();
        }
        protected override void DataOperation()
        {
            // for now we do not check this challenge correction
            _session.PlayerData.SessionKey = 2020;
            _session.PlayerData.GameName = _request.GameName;
        }

        protected override void ResponseConstruct()
        {
            _response = new AuthResponse(_request, _result);
        }
    }
}
