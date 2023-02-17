
using UniSpy.Server.WebServer.Module.Direct2Game.Abstraction;
using UniSpy.Server.WebServer.Module.Direct2Game.Contract.Result;

namespace UniSpy.Server.WebServer.Module.Direct2Game.Contract.Response
{
    public class GetPurchaseHistoryResponse : ResponseBase
    {
        private new GetPurchaseHistoryResult _result => (GetPurchaseHistoryResult)base._result;

        public GetPurchaseHistoryResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            _content.Add("GetPurchaseHistoryResult");
            _content.Add("status");
            _content.Add("code", _result.Status);
            _content.ChangeToElement("GetPurchaseHistoryResult");
            _content.Add("orderpurchases");
            _content.Add("count", 0);
            base.Build();
        }
    }
}
