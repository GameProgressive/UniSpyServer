using GameSpyLib.Extensions;
using QueryReport.Abstraction.BaseClass;
using System.Net;

namespace QueryReport.Entity.Structure.Request
{
    public class ChallengeRequest : QRRequestBase
    {
        public string RemoteIP { get; protected set; }
        public string RemotePort { get; protected set; }

        public ChallengeRequest(EndPoint endPoint,byte[] rawRequest) : base(rawRequest)
        {
            RemoteIP = HtonsExtensions.EndPointToIPString(endPoint);
            RemotePort = HtonsExtensions.EndPointToPortString(endPoint);
        }
    }
}
