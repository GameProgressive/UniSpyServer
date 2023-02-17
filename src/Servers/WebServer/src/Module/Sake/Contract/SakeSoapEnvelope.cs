using System.Xml.Linq;
using UniSpy.Server.WebServer.Abstraction;

namespace UniSpy.Server.WebServer.Module.Sake.Contract
{
    public sealed class SakeSoapEnvelope : SoapEnvelopBase
    {
        public static XNamespace SakeNamespace = "http://gamespy.net/sake";
        public SakeSoapEnvelope() : base("ns1", SakeNamespace)
        {
        }
    }
}