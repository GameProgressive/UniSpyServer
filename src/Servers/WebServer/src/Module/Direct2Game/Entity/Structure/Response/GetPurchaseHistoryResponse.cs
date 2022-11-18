using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Structure;
using UniSpyServer.Servers.WebServer.Module.Direct2Game.Entity.Structure.Result;

namespace UniSpyServer.Servers.WebServer.Module.Direct2Game.Entity.Structure.Response
{
    public class GetPurchaseHistoryResponse : ResponseBase
    {
        private new GetPurchaseHistoryResult _result => (GetPurchaseHistoryResult)base._result;

        public GetPurchaseHistoryResponse(RequestBase request, ResultBase result) : base(request, result)
        {
            _content = new Direct2GameSoapEnvelope();
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
