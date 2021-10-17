using System;
using System.Xml.Linq;

namespace WebServer.Entity.Structure
{
    public class SoapXElement : XElement
    {
        private static XDeclaration _declaration = new XDeclaration("1.0", "utf-8", null);
        public SoapXElement(XElement other) : base(other) { }

        public SoapXElement(XName name) : base(name) { }

        public SoapXElement(XStreamingElement other) : base(other) { }

        public SoapXElement(XName name, object content) : base(name, content) { }

        public SoapXElement(XName name, params object[] content) : base(name, content) { }

        public override string ToString()
        {
            return _declaration + Environment.NewLine + base.ToString();
        }
    }
}