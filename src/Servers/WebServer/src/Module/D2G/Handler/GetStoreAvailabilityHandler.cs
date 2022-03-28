using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.Servers.WebServer.Module.D2G.Entity.Structure.Result;
using UniSpyServer.Servers.WebServer.Module.D2G.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.WebServer.Module.D2G.Handler
{
    [HandlerContract("GetStoreAvailability")]
    public class GetStoreAvailabilityHandler : CmdHandlerBase
    {
        protected new GetStoreAvailabilityRequest _request => (GetStoreAvailabilityRequest)base._request;
        protected new GetStoreAvailabilityResult _result = new GetStoreAvailabilityResult();

        public GetStoreAvailabilityHandler(IClient client, IRequest request) : base(client, request)
        {

        }

        protected override void DataOperation()
        {
            // TODO
            _result.Status = 0;
            _result.StoreResult = 100;
        }

        protected override void ResponseConstruct()
        {
            _response = new GetStoreAvailabilityResponse(_request, _result);
        }

    }
}