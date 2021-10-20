using System.IO;
using System.Xml.Linq;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.WebServer.Abstraction
{
    public abstract class RequestBase : UniSpyRequestBase
    {
        public new string RawRequest => (string)base.RawRequest;
        protected XElement _contentElement { get; private set; }
        public RequestBase(string rawRequest) : base(rawRequest)
        {
            dynamic xelements = XElement.Parse(RawRequest);
            _contentElement = xelements.FirstNode.FirstNode;
        }

        protected RequestBase()
        {
        }
    }
}