using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Structure;

namespace UniSpyServer.Servers.WebServer.Module.Direct2Game.Entity.Structure.Response
{
    public class GetPurchaseHistoryResponse : ResponseBase
    {
        public GetPurchaseHistoryResponse(RequestBase request, ResultBase result) : base(request, result)
        {
            _content = new Direct2GameSoapEnvelope();
        }

        public override void Build()
        {
            _content.Add("GetPurchaseHistoryResult");
            _content.Add("status");
            _content.Add("code", 0);
            _content.ChangeToElement("GetPurchaseHistoryResult");
            _content.Add("orderpurchases");
            _content.Add("accountid", 11);
            _content.Add("culturecode", "en");
            _content.Add("currencycode", "en");
            _content.Add("subtotal", "10.00");
            _content.Add("tax", "0.00");
            _content.Add("rootorderid", "0");
            base.Build();
        }
    }
}
