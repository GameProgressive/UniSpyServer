using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Response
{
    public sealed class AuthPlayerResponse : ResponseBase
    {
        private new AuthPlayerResult _result => (AuthPlayerResult)base._result;
        private new AuthPlayerRequest _request => (AuthPlayerRequest)base._request;
        public AuthPlayerResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\pauthr\{_result.ProfileID}\lid\{ _request.OperationID}\final\";
        }
    }
}
