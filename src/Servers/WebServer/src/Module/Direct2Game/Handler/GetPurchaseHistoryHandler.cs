using UniSpy.Server.WebServer.Abstraction;
using UniSpy.Server.WebServer.Module.Direct2Game.Contract.Request;
using UniSpy.Server.WebServer.Module.Direct2Game.Contract.Response;
using UniSpy.Server.WebServer.Module.Direct2Game.Contract.Result;
using UniSpy.Server.WebServer.Application;

namespace UniSpy.Server.WebServer.Module.Direct2Game.Handler
{

    internal class GetPurchaseHistoryHandler : CmdHandlerBase
    {
        protected new GetPurchaseHistoryRequest _request => (GetPurchaseHistoryRequest)base._request;
        protected new GetPurchaseHistoryResult _result = new GetPurchaseHistoryResult();

        public GetPurchaseHistoryHandler(Client client, GetPurchaseHistoryRequest request) : base(client, request)
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
