using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.Entity.Enumerate;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Result
{
    public sealed class ChallengeResult : ResultBase
    {
        public ChallengeResult()
        {
            PacketType = Enumerate.PacketType.Challenge;
        }
    }
}
