using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Response
{
    public sealed class AuthResponse : ResponseBase
    {
        private new AuthResult _result => (AuthResult)base._result;
        private new AuthGameRequest _request => (AuthGameRequest)base._request;
        public AuthResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\sesskey\{_result.SessionKey}\lid\{ _request.OperationID}\final\";
        }
    }
}
