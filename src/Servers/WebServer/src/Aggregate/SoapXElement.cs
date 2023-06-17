using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace UniSpy.Server.WebServer.Aggregate
{
    public class SoapXElement : XElement
    {
        private static XDeclaration _declaration = new XDeclaration("1.0", "utf-8", null);
        public static XNamespace SoapEnvelopNamespace = "http://schemas.xmlsoap.org/soap/envelope/";
        public static readonly XElement SoapHeader = new XElement(SoapEnvelopNamespace + "Envelope",
                                new XAttribute(XNamespace.Xmlns + "SOAP-ENV", SoapEnvelopNamespace),
                                new XAttribute(XNamespace.Xmlns + "SOAP-ENC", "http://schemas.xmlsoap.org/soap/encoding/"),
                                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                                new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"));
        public static XNamespace SakeNamespace = "http://gamespy.net/sake";
        public static XElement SakeSoapHeader
        {
            get
            {
                var element = new XElement(SoapHeader);
                element.Add(new XAttribute(XNamespace.Xmlns + "ns1", SakeNamespace));
                return element;
            }
        }
        public static XNamespace AuthNamespace = "http://gamespy.net/AuthService/";
        public static XElement AuthSoapHeader
        {
            get
            {
                var element = new XElement(SoapHeader);
                element.Add(new XAttribute(XNamespace.Xmlns + "ns1", AuthNamespace));
                return element;
            }
        }
        public static XNamespace Direct2GameNamespace = "http://gamespy.net/commerce/2009/02";
        public static XElement Direct2GameSoapHeader
        {
            get
            {
                var element = new XElement(SoapHeader);
                element.Add(new XAttribute(XNamespace.Xmlns + "gsc", Direct2GameNamespace));
                return element;
            }
        }
        public SoapXElement(XElement other) : base(other) { }
        public SoapXElement(XName name) : base(name) { }
        public SoapXElement(XName name, object content) : base(name, content) { }
        public SoapXElement(XName name, params object[] content) : base(name, content) { }
        public List<FieldObject> GetArrayObjects()
        {
            throw new NotImplementedException();
        }
        public XElement SetArrayObjects(List<FieldObject> objects)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return _declaration + Environment.NewLine + base.ToString();
        }
    }
}