using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.Servers.WebServer.Module.D2G.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.WebServer.Module.D2G.Handler
{
    [HandlerContract("GetStoreAvailability")]
    public class GetStoreAvailabilityHandler : CmdHandlerBase
    {
        protected new GetStoreAvailabilityRequest _request => (GetStoreAvailabilityRequest)base._request;
        public GetStoreAvailabilityHandler(IClient client, IRequest request) : base(client, request)
        {
        }
    }
}