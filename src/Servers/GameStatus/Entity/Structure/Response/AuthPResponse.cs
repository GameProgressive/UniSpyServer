using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Structure.Request;
using GameStatus.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace GameStatus.Entity.Structure.Response
{
    internal sealed class AuthPResponse : GSResponseBase
    {
        private new AuthPResult _result => (AuthPResult)base._result;
        private new AuthPRequest _request => (AuthPRequest)base._request;
        public AuthPResponse(UniSpyRequest request, UniSpyResult result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\pauthr\{_result.ProfileID}\lid\{ _request.OperationID}\final\";
        }
    }
}
