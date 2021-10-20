using UniSpyServer.QueryReport.Abstraction.BaseClass;
using UniSpyServer.QueryReport.Entity.contract;
using UniSpyServer.QueryReport.Entity.Enumerate;

namespace UniSpyServer.QueryReport.Entity.Structure.Request
{
    [RequestContract(RequestType.Challenge)]
    public sealed class ChallengeRequest : RequestBase
    {
        public ChallengeRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
