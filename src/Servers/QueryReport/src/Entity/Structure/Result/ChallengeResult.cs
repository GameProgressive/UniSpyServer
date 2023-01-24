using UniSpyServer.Servers.QueryReport.V2.Abstraction.BaseClass;

namespace UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Result
{
    public sealed class ChallengeResult : ResultBase
    {
        public ChallengeResult()
        {
            PacketType = Enumerate.PacketType.Challenge;
        }
    }
}
