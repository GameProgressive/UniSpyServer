using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Structure;

namespace UniSpyServer.Servers.WebServer.Module.D2G.Entity.Structure.Response
{
    public class GetPurchaseHistoryResponse : ResponseBase
    {
        public GetPurchaseHistoryResponse(RequestBase request, ResultBase result) : base(request, result)
        {
            //_result = (GetPurchaseHistoryResponse)result;
            _soapEnvelop = new SoapXElement(SoapXElement.D2GSoapHeader);
            _soapBody = new XElement(SoapXElement.SoapNamespace + "Body");
        }

        public override void Build()
        {
            _soapBody.Add(new XElement(SoapXElement.D2GNamespace + "GetPurchaseHistoryResult"));
            var e = _soapBody.Elements().First();
            e.Add(new XElement(SoapXElement.D2GNamespace + "status"));
            e.Elements().First().Add(new XElement(SoapXElement.D2GNamespace + "code", 0));
            e.Add(new XElement(SoapXElement.D2GNamespace + "orderpurchases"));
            var oe = e.Elements().First().ElementsAfterSelf().First();
            oe.Add(new XElement(SoapXElement.D2GNamespace + "count", 1));
            oe.Add(new XElement(SoapXElement.D2GNamespace + "orderpurchase"));
            var oe2 = oe.ElementsAfterSelf().First();
            oe2.Add(new XElement(SoapXElement.D2GNamespace + "accountid", 11));
            oe2.Add(new XElement(SoapXElement.D2GNamespace + "culturecode", "en"));
            oe2.Add(new XElement(SoapXElement.D2GNamespace + "currencycode", "en"));
            oe2.Add(new XElement(SoapXElement.D2GNamespace + "subtotal", "10.00"));
            oe2.Add(new XElement(SoapXElement.D2GNamespace + "tax", "0.00"));
            oe2.Add(new XElement(SoapXElement.D2GNamespace + "rootorderid", "0"));
            base.Build();
        }
    }
}
