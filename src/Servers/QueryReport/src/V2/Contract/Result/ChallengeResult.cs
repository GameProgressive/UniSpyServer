using UniSpy.Server.QueryReport.V2.Abstraction.BaseClass;

namespace UniSpy.Server.QueryReport.V2.Contract.Result
{
    public sealed class ChallengeResult : ResultBase
    {
        public ChallengeResult()
        {
            PacketType = Enumerate.PacketType.Challenge;
        }
    }
}
