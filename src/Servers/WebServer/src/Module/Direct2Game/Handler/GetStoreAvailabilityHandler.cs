using UniSpy.Server.WebServer.Abstraction;
using UniSpy.Server.WebServer.Module.Direct2Game.Contract.Request;
using UniSpy.Server.WebServer.Module.Direct2Game.Contract.Result;
using UniSpy.Server.WebServer.Application;

namespace UniSpy.Server.WebServer.Module.Direct2Game.Handler
{
    public class GetStoreAvailabilityHandler : CmdHandlerBase
    {
        protected new GetStoreAvailabilityRequest _request => (GetStoreAvailabilityRequest)base._request;
        protected new GetStoreAvailabilityResult _result = new GetStoreAvailabilityResult();
        public GetStoreAvailabilityHandler(Client client, GetStoreAvailabilityRequest request) : base(client, request)
        {
        }

        protected override void ResponseConstruct()
        {
            _response = new GetStoreAvailabilityResponse(_request, _result);
        }
    }
}