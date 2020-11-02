using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;

namespace QueryReport.Entity.Structure.Request
{
    public class ChallengeRequest : QRRequestBase
    {
        public ChallengeRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
