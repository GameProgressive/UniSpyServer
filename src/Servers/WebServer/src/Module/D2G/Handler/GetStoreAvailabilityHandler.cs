using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.Servers.WebServer.Module.D2G.Entity.Result;
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
            _result.Status = 0;
            _result.StoreResult = 50;
            /*
             *  10 -> Store is available
             *  50 -> ERROR
             *  100 -> Cannot download DLC information from backend server
             */
        }

        protected override void ResponseConstruct()
        {
            _response = new GetStoreAvailabilityResponse(_request, _result);
        }

    }
}