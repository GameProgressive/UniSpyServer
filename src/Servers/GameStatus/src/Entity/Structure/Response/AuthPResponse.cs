using UniSpyServer.GameStatus.Abstraction.BaseClass;
using UniSpyServer.GameStatus.Entity.Structure.Request;
using UniSpyServer.GameStatus.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.GameStatus.Entity.Structure.Response
{
    public sealed class AuthPResponse : ResponseBase
    {
        private new AuthPResult _result => (AuthPResult)base._result;
        private new AuthPlayerRequest _request => (AuthPlayerRequest)base._request;
        public AuthPResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\pauthr\{_result.ProfileID}\lid\{ _request.OperationID}\final\";
        }
    }
}
