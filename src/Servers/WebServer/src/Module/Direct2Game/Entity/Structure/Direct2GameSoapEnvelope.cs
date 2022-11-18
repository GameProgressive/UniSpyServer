using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Direct2Game.Entity.Structure
{
    public sealed class Direct2GameSoapEnvelope : SoapEnvelopBase
    {
        public static XNamespace Direct2GameNamespace = "http://gamespy.net/commerce/2009/02";
        public Direct2GameSoapEnvelope() : base("gsc", Direct2GameNamespace)
        {
        }
    }
}