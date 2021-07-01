using QueryReport.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace QueryReport.Abstraction.BaseClass
{
    internal abstract class QRResultBase : UniSpyResult
    {
        public QRPacketType? PacketType { get; protected set; }
        public QRResultBase()
        {
        }
    }
}