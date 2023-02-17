using System.Xml.Linq;
using UniSpy.Server.WebServer.Abstraction;

namespace UniSpy.Server.WebServer.Module.Direct2Game.Contract
{
    public sealed class Direct2GameSoapEnvelope : SoapEnvelopBase
    {
        public static XNamespace Direct2GameNamespace = "http://gamespy.net/commerce/2009/02";
        public Direct2GameSoapEnvelope() : base("gsc", Direct2GameNamespace)
        {
        }
    }
}