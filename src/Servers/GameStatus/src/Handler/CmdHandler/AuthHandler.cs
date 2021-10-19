using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Contract;
using GameStatus.Entity.Structure.Request;
using GameStatus.Entity.Structure.Response;
using GameStatus.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;

namespace GameStatus.Handler.CmdHandler
{
    [HandlerContract("auth")]
    public sealed class AuthHandler : CmdHandlerBase
    {
        //UniSpyLib.Encryption.Crc16 _crc16 = new UniSpyLib.Encryption.Crc16(UniSpyLib.Encryption.Crc16Mode.Standard);
        private new AuthGameRequest _request => (AuthGameRequest)base._request;
        private new AuthResult _result
        {
            get => (AuthResult)base._result;
            set => base._result = value;
        }
        public AuthHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
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
