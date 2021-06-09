using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Structure.Request;
using GameStatus.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace GameStatus.Entity.Structure.Response
{
    internal sealed class GetPIDResponse : GSResponseBase
    {
        private new GetPIDResult _result => (GetPIDResult)base._result;
        private new GetPIDRequest _request => (GetPIDRequest)base._request;
        public GetPIDResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\getpidr\{_result.ProfileID}\lid\{ _request.OperationID}\final\";
        }
    }
}
