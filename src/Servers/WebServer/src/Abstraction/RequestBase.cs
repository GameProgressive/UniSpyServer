using System.Xml.Linq;

namespace UniSpy.Server.WebServer.Abstraction
{
    public abstract class RequestBase : UniSpy.Server.Core.Abstraction.BaseClass.RequestBase
    {
        public new string RawRequest => (string)base.RawRequest;
        protected XElement _contentElement { get; private set; }
        public RequestBase(string rawRequest) : base(rawRequest)
        {
        }

        protected RequestBase()
        {
        }
        public override void Parse()
        {
            dynamic xelements = XElement.Parse(RawRequest);
            _contentElement = xelements.FirstNode.FirstNode;
        }
    }
}