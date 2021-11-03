using System.Xml.Linq;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.Servers.WebServer.Entity.Structure;

namespace UniSpyServer.Servers.WebServer.Abstraction
{
    public abstract class ResponseBase : UniSpyResponseBase
    {
        protected SoapXElement _xElement { get; private set; }
        public ResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            // Because the response is kind of soap object, so we did not use SoapXElement as a soap object
            // SoapXElement only acts like XElement
            _xElement.Add(SoapXElement.SoapElement);
            _xElement.Add(new XElement(SoapXElement.SoapNamespace + "Body"));
        }
    }
}