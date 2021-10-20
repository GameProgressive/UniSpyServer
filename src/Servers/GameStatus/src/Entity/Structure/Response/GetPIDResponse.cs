using UniSpyServer.GameStatus.Abstraction.BaseClass;
using UniSpyServer.GameStatus.Entity.Structure.Request;
using UniSpyServer.GameStatus.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.GameStatus.Entity.Structure.Response
{
    public sealed class GetPIDResponse : ResponseBase
    {
        private new GetPIDResult _result => (GetPIDResult)base._result;
        private new GetProfileIDRequest _request => (GetProfileIDRequest)base._request;
        public GetPIDResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\getpidr\{_result.ProfileID}\lid\{ _request.OperationID}\final\";
        }
    }
}
