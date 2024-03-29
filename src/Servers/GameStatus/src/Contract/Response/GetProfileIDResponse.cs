using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Contract.Request;
using UniSpy.Server.GameStatus.Contract.Result;

namespace UniSpy.Server.GameStatus.Contract.Response
{
    public sealed class GetProfileIDResponse : ResponseBase
    {
        private new GetProfileIDResult _result => (GetProfileIDResult)base._result;
        private new GetProfileIDRequest _request => (GetProfileIDRequest)base._request;
        public GetProfileIDResponse(GetProfileIDRequest request, GetProfileIDResult result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\getpidr\{_result.ProfileId}\lid\{ _request.LocalId}\final\";
        }
    }
}
