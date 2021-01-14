using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;

namespace QueryReport.Entity.Structure.Result
{
    internal sealed class ChallengeResult : QRResultBase
    {
        public ChallengeResult()
        {
            PacketType = QRPacketType.Challenge;
        }
    }
}
