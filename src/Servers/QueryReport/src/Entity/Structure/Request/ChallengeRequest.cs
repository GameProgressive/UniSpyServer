using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.contract;
using QueryReport.Entity.Enumerate;

namespace QueryReport.Entity.Structure.Request
{
    [RequestContract(RequestType.Challenge)]
    internal sealed class ChallengeRequest : RequestBase
    {
        public ChallengeRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
