using UniSpyLib.Extensions;
using QueryReport.Abstraction.BaseClass;
using System.Net;

namespace QueryReport.Entity.Structure.Request
{
    public class HeartBeatRequest : QRRequestBase
    {
        public HeartBeatRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
