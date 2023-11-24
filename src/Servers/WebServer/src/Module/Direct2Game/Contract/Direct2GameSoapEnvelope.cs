using System.Xml.Linq;
using UniSpy.Server.WebServer.Abstraction;

namespace UniSpy.Server.WebServer.Module.Direct2Game.Contract
{
    public sealed class Direct2GameSoapEnvelope : SoapEnvelopBase
    {
        public static XNamespace Direct2GameNamespace = "http://gamespy.net/commerce/";
        public Direct2GameSoapEnvelope() : base("ns1", Direct2GameNamespace)
        {
        }
    }
}