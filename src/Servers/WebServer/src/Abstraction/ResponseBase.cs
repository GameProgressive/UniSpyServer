using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Entity.Structure;

namespace UniSpyServer.Servers.WebServer.Abstraction
{
    public abstract class ResponseBase : UniSpyLib.Abstraction.BaseClass.ResponseBase
    {
        protected SoapXElement _soapElement { get; private set; }
        public new string SendingBuffer => _soapElement.ToString();
        public ResponseBase(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
            _soapElement = new SoapXElement(SoapXElement.SoapElement);
        }
        public override void Build()
        {
            // Because the response is kind of soap object, so we did not use SoapXElement as a soap object
            // SoapXElement only acts like XElement
            _soapElement.Add(new XElement(SoapXElement.SoapNamespace + "Body"));
        }
    }
}