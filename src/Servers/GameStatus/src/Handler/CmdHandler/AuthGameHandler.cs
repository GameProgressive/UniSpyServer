using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Contract.Request;
using UniSpy.Server.GameStatus.Contract.Response;
using UniSpy.Server.GameStatus.Contract.Result;
using UniSpy.Server.GameStatus.Application;

namespace UniSpy.Server.GameStatus.Handler.CmdHandler
{

    public sealed class AuthGameHandler : CmdHandlerBase
    {
        private new AuthGameRequest _request => (AuthGameRequest)base._request;
        private new AuthGameResult _result { get => (AuthGameResult)base._result; set => base._result = value; }
        public AuthGameHandler(Client client, AuthGameRequest request) : base(client, request)
        {
            _result = new AuthGameResult();
        }
        protected override void DataOperation()
        {
            // for now we do not check this challenge correction
            _client.Info.SessionKey = 2020;
            _client.Info.GameName = _request.GameName;
            _client.Info.IsGameAuthenticated = true;
        }

        protected override void ResponseConstruct()
        {
            _response = new AuthGameResponse(_request, _result);
        }
    }
}
