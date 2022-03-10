using System.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Abstraction
{
    public abstract class LoginRequestBase : RequestBase
    {
        protected LoginRequestBase(string rawRequest) : base(rawRequest)
        {
        }

        public int Version { get; private set; }
        public int PartnerCode { get; private set; }
        public int NamespaceId { get; private set; }
        public override void Parse()
        {
            base.Parse();
            var version = _contentElement.Descendants().First(p => p.Name.LocalName == "version").Value;
            Version = int.Parse(version);
            var partnercode = _contentElement.Descendants().First(p => p.Name.LocalName == "partnercode").Value;
            PartnerCode = int.Parse(partnercode);
            var namespaceid = _contentElement.Descendants().First(p => p.Name.LocalName == "namespaceid").Value;
            NamespaceId = int.Parse(namespaceid);
        }
    }
}