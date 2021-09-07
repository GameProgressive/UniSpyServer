using QueryReport.Abstraction.BaseClass;

namespace QueryReport.Entity.Structure.Request
{
    internal sealed class ChallengeRequest : RequestBase
    {
        public ChallengeRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
