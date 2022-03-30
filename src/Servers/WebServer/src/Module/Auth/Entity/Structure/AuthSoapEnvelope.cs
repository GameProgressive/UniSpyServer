using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Structure;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure
{
    public sealed class AuthSoapEnvelope : SoapEnvelopBase
    {
        public static XNamespace AuthNamespace = "http://gamespy.net/AuthService/";
        public AuthSoapEnvelope() : base("ns1", AuthNamespace)
        {
        }
    }
}