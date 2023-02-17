using UniSpy.Server.WebServer.Abstraction;
using UniSpy.Server.WebServer.Module.Direct2Game.Entity.Structure.Request;
using UniSpy.Server.WebServer.Module.Direct2Game.Entity.Structure.Response;
using UniSpy.Server.WebServer.Module.Direct2Game.Entity.Structure.Result;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.WebServer.Module.Direct2Game.Handler
{
    
    internal class GetPurchaseHistoryHandler : CmdHandlerBase
    {
        protected new GetPurchaseHistoryRequest _request => (GetPurchaseHistoryRequest)base._request;
        protected new GetPurchaseHistoryResult _result = new GetPurchaseHistoryResult();

        public GetPurchaseHistoryHandler(IClient client, IRequest request) : base(client, request)
        {

        }

        protected override void DataOperation()
        {

        }

        protected override void ResponseConstruct()
        {
            _result.Status = 0;

            _response = new GetPurchaseHistoryResponse(_request, _result);
        }
    }
}
