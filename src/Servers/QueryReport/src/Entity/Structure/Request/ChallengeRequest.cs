using UniSpyServer.Servers.QueryReport.V2.Abstraction.BaseClass;

namespace UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Request
{

    public sealed class ChallengeRequest : RequestBase
    {
        public ChallengeRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
