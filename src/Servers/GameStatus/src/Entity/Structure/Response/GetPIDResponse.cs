using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Request;
using UniSpyServer.Servers.GameStatus.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Response
{
    public sealed class GetPIDResponse : ResponseBase
    {
        private new GetProfileIDResult _result => (GetProfileIDResult)base._result;
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
