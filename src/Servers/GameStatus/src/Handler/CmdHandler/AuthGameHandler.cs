using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Contract.Request;
using UniSpy.Server.GameStatus.Contract.Response;
using UniSpy.Server.GameStatus.Contract.Result;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.GameStatus.Handler.CmdHandler
{
    
    public sealed class AuthGameHandler : CmdHandlerBase
    {
        //UniSpy.Server.Core.Encryption.Crc16 _crc16 = new UniSpy.Server.Core.Encryption.Crc16(UniSpy.Server.Core.Encryption.Crc16Mode.Standard);
        private new AuthGameRequest _request => (AuthGameRequest)base._request;
        private new AuthGameResult _result { get => (AuthGameResult)base._result; set => base._result = value; }
        public AuthGameHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new AuthGameResult();
        }
        protected override void DataOperation()
        {
            // for now we do not check this challenge correction
            _client.Info.SessionKey = 2020;
            _client.Info.GameName = _request.GameName;
        }

        protected override void ResponseConstruct()
        {
            _response = new AuthGameResponse(_request, _result);
        }
    }
}
