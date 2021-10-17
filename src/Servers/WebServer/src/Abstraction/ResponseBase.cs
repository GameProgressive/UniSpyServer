using System.Xml.Linq;
using UniSpyLib.Abstraction.BaseClass;
using WebServer.Entity.Structure;

namespace WebServer.Abstraction
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