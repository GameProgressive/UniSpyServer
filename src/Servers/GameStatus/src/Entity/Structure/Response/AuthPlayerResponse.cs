using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Result;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Response
{
    public sealed class AuthPlayerResponse : ResponseBase
    {
        private new AuthPlayerResult _result => (AuthPlayerResult)base._result;
        private new AuthPlayerRequest _request => (AuthPlayerRequest)base._request;
        public AuthPlayerResponse(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\pauthr\{_result.ProfileId}\lid\{ _request.OperationID}\final\";
        }
    }
}
