using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Structure;

namespace UniSpyServer.Servers.WebServer.Module.Sake.Abstraction
{
    public abstract class ResponseBase : WebServer.Abstraction.ResponseBase
    {
        public ResponseBase(RequestBase request, ResultBase result) : base(request, result)
        {
            _soapEnvelop = new SoapXElement(SoapXElement.SakeSoapHeader);
            _soapBody = new XElement(SoapXElement.SakeNamespace + "Body");
        }
    }
}