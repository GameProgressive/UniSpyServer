using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Result;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Response
{
    public sealed class GetProfileIDResponse : ResponseBase
    {
        private new GetProfileIDResult _result => (GetProfileIDResult)base._result;
        private new GetProfileIDRequest _request => (GetProfileIDRequest)base._request;
        public GetProfileIDResponse(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\getpidr\{_result.ProfileId}\lid\{ _request.OperationID}\final\";
        }
    }
}
