using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace UniSpy.Server.WebServer.Abstraction
{
    public abstract class SoapEnvelopBase
    {
        private static XDeclaration _declaration = new XDeclaration("1.0", "utf-8", null);
        public static XNamespace SoapEnvelopNamespace = "http://schemas.xmlsoap.org/soap/envelope/";
        public XElement SoapHeader = new XElement(SoapEnvelopNamespace + "Envelope",
                                new XAttribute(XNamespace.Xmlns + "SOAP-ENV", SoapEnvelopNamespace),
                                new XAttribute(XNamespace.Xmlns + "SOAP-ENC", "http://schemas.xmlsoap.org/soap/encoding/"),
                                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                                new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"));
        private XNamespace _bodyNamespace;
        public XDocument Content { get; private set; }
        public XElement Body => Content.Root.Descendants().First(p => p.Name.LocalName == "Body");
        public XElement CurrentElement { get; private set; }
        public SoapEnvelopBase(string nsShortName, XNamespace bodyNamespace)
        {
            _bodyNamespace = bodyNamespace;
            Content = new XDocument(_declaration);
            Content.Declaration = _declaration;
            SoapHeader.Add(new XAttribute(XNamespace.Xmlns + nsShortName, _bodyNamespace));
            Content.Add(SoapHeader);
            Content.Root.Add(new XElement(SoapEnvelopNamespace + "Body"));
            CurrentElement = Body;
        }
        public void FinishAddSubElement()
        {
            CurrentElement = Body;
        }
        public void ChangeToElement(string name)
        {
            CurrentElement = Content.Descendants().First(p => p.Name.LocalName == name);
        }
        public void BackToParentElement()
        {
            CurrentElement = CurrentElement.Parent;
        }
        public virtual void Add(string name)
        {
            CurrentElement.Add(new XElement(_bodyNamespace + name));
            ChangeToElement(name);
        }
        public virtual void Add(string name, object value)
        {
            CurrentElement.Add(new XElement(_bodyNamespace + name, value));
        }
        // public virtual void Add(string name, List<RecordFieldObject> values)
        // {
        //     throw new NotImplementedException();
        // }
        // public virtual void Add(string name, List<FieldObject> values)
        // {
        //     throw new NotImplementedException();
        // }

        public override string ToString()
        {
            using (var writer = new MyStringWriter())
            {
                Content.Save(writer);
                return writer.ToString();
            }
        }

        private class MyStringWriter : StringWriter
        {
            public MyStringWriter()
            {
                Encoding = Encoding.UTF8;
            }

            public override Encoding Encoding { get; }
        }
    }

}