using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Sake.Entity.Structure
{
    public sealed class SakeSoapEnvelope : SoapEnvelopBase
    {
        public static XNamespace SakeNamespace = "http://gamespy.net/sake";
        public SakeSoapEnvelope() : base("ns1", SakeNamespace)
        {
        }
    }
}