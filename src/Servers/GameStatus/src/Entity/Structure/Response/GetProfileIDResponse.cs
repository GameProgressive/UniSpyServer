using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Entity.Structure.Request;
using UniSpy.Server.GameStatus.Entity.Structure.Result;

namespace UniSpy.Server.GameStatus.Entity.Structure.Response
{
    public sealed class GetProfileIDResponse : ResponseBase
    {
        private new GetProfileIDResult _result => (GetProfileIDResult)base._result;
        private new GetProfileIDRequest _request => (GetProfileIDRequest)base._request;
        public GetProfileIDResponse(UniSpy.Server.Core.Abstraction.BaseClass.RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\getpidr\{_result.ProfileId}\lid\{ _request.OperationID}\final\";
        }
    }
}
