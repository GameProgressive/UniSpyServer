using UniSpy.Server.QueryReport.Abstraction.BaseClass;

namespace UniSpy.Server.QueryReport.Contract.Request
{

    public sealed class ChallengeRequest : RequestBase
    {
        public ChallengeRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
