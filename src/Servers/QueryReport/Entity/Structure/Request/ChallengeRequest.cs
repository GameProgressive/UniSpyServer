using QueryReport.Abstraction.BaseClass;

namespace QueryReport.Entity.Structure.Request
{
    internal sealed class ChallengeRequest : QRRequestBase
    {
        public ChallengeRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
