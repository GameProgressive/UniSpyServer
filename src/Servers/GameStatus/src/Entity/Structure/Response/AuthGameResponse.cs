using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Result;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Response
{
    public sealed class AuthGameResponse : ResponseBase
    {
        private new AuthGameResult _result => (AuthGameResult)base._result;
        private new AuthGameRequest _request => (AuthGameRequest)base._request;
        public AuthGameResponse(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\sesskey\{_result.SessionKey}\lid\{ _request.OperationID}\final\";
        }
    }
}
