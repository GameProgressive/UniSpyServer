using System.Linq;
using UniSpy.Server.WebServer.Abstraction;


namespace UniSpy.Server.WebServer.Module.Auth.Abstraction
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
            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "version"))
            {
                throw new Auth.Exception("version is missing from the request");
            }
            var version = _contentElement.Descendants().First(p => p.Name.LocalName == "version").Value;
            Version = int.Parse(version);

            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "partnercode"))
            {
                throw new Auth.Exception("partnercode is missing from the request");
            }
            var partnercode = _contentElement.Descendants().First(p => p.Name.LocalName == "partnercode").Value;
            PartnerCode = int.Parse(partnercode);

            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "namespaceid"))
            {
                throw new Auth.Exception("namespaceid is missing from the request");
            }
            var namespaceid = _contentElement.Descendants().First(p => p.Name.LocalName == "namespaceid").Value;
            NamespaceId = int.Parse(namespaceid);
        }
    }
}