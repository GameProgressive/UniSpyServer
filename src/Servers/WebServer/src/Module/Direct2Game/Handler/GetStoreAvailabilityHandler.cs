using UniSpy.Server.WebServer.Abstraction;
using UniSpy.Server.WebServer.Module.Direct2Game.Contract.Request;
using UniSpy.Server.WebServer.Module.Direct2Game.Contract.Result;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.WebServer.Module.Direct2Game.Handler
{
    
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
            _result.StoreResult = 10;
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