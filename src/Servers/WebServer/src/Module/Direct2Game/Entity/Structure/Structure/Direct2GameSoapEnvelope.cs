using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;

namespace UniSpyServer.Servers.WebServer.Entity.Structure
{
    public sealed class Direct2GameSoapEnvelope : SoapEnvelopBase
    {
        public static XNamespace Direct2GameNamespace = "http://gamespy.net/Direct2Game";
        public Direct2GameSoapEnvelope() : base("gsc", Direct2GameNamespace)
        {
        }
    }
}