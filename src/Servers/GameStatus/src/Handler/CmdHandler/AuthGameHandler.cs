using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Response;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.GameStatus.Handler.CmdHandler
{
    
    public sealed class AuthGameHandler : CmdHandlerBase
    {
        //UniSpyServer.UniSpyLib.Encryption.Crc16 _crc16 = new UniSpyServer.UniSpyLib.Encryption.Crc16(UniSpyServer.UniSpyLib.Encryption.Crc16Mode.Standard);
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
