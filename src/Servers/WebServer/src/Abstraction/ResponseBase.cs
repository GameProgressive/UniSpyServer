using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using UniSpyLib.Abstraction.BaseClass;

namespace WebServer.Abstraction
{
    public abstract class ResponseBase : UniSpyResponseBase
    {
        protected XDocument _doc { get; private set; }
        protected static XNamespace _ns1 = "http://gamespy.net/sake";
        public ResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            _doc = new XDocument();
            XNamespace soapEnv = "http://schemas.xmlsoap.org/soap/envelope/";
            _doc.Add(new XElement(soapEnv + "Envelope",
                                new XAttribute(XNamespace.Xmlns + "SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/"),
                                new XAttribute(XNamespace.Xmlns + "SOAP-ENC", "http://schemas.xmlsoap.org/soap/encoding/"),
                                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                                new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema")));
            _doc.Add(new XElement(soapEnv + "Body"));
        }
    }
}