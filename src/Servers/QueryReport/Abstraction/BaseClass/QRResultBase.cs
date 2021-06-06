using QueryReport.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace QueryReport.Abstraction.BaseClass
{
    internal abstract class QRResultBase : UniSpyResultBase
    {
        public new QRErrorCode ErrorCode
        {
            get => (QRErrorCode)base.ErrorCode;
            set => base.ErrorCode = value;
        }
        public QRPacketType? PacketType { get; protected set; }
        public QRResultBase()
        {
            ErrorCode = QRErrorCode.NoError;
        }
    }
}