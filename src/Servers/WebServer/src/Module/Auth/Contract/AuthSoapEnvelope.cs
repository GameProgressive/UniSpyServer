using System.Xml.Linq;
using UniSpy.Server.WebServer.Abstraction;

namespace UniSpy.Server.WebServer.Module.Auth.Contract
{
    public sealed class AuthSoapEnvelope : SoapEnvelopBase
    {
        public static XNamespace AuthNamespace = "http://gamespy.net/AuthService/";
        public AuthSoapEnvelope() : base("ns1", AuthNamespace)
        {
        }
    }
}