using UniSpyServer.QueryReport.Abstraction.BaseClass;
using UniSpyServer.QueryReport.Entity.Enumerate;

namespace UniSpyServer.QueryReport.Entity.Structure.Result
{
    public sealed class ChallengeResult : ResultBase
    {
        public ChallengeResult()
        {
            PacketType = Enumerate.PacketType.Challenge;
        }
    }
}
