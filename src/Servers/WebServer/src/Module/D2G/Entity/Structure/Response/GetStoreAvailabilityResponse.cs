using System.Linq;
using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Structure;
using UniSpyServer.Servers.WebServer.Module.D2G.Entity.Structure.Result;

namespace UniSpyServer.Servers.WebServer.Module.D2G.Entity.Structure.Request
{
    public class GetStoreAvailabilityResponse : ResponseBase
    {
        private new GetStoreAvailabilityResult _result;

        public GetStoreAvailabilityResponse(RequestBase request, ResultBase result) : base(request, result)
        {
            _result = (GetStoreAvailabilityResult)result;
            _soapEnvelop = new SoapXElement(SoapXElement.D2GSoapHeader);
            _soapBody = new XElement(SoapXElement.SoapNamespace + "Body");
        }

        public override void Build()
        {
            _soapBody.Add(new XElement(SoapXElement.D2GNamespace + "GetStoreAvailabilityResult"));
            var e = _soapBody.Elements().First();
            e.Add(new XElement(SoapXElement.D2GNamespace + "status"));
            e.Elements().First().Add(new XElement(SoapXElement.D2GNamespace + "code", _result.Status));
            e.Add(new XElement(SoapXElement.D2GNamespace + "storestatusid", _result.StoreResult));
            base.Build();
        }
    }
}
