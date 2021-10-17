using System.Xml.Linq;
using UniSpyLib.Abstraction.BaseClass;
using WebServer.Entity.Structure;

namespace WebServer.Abstraction
{
    public abstract class ResponseBase : UniSpyResponseBase
    {
        protected SoapXElement _xElement { get; private set; }
        protected static XDeclaration _declaration = new XDeclaration("1.0", "utf-8", null);
        protected static XNamespace _ns1 = "http://gamespy.net/sake";
        public ResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            XNamespace soapEnv = "http://schemas.xmlsoap.org/soap/envelope/";
            _xElement.Add(new XElement(soapEnv + "Envelope",
                                new XAttribute(XNamespace.Xmlns + "SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/"),
                                new XAttribute(XNamespace.Xmlns + "SOAP-ENC", "http://schemas.xmlsoap.org/soap/encoding/"),
                                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                                new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema")));
            _xElement.Add(new XElement(soapEnv + "Body"));
        }
    }
}