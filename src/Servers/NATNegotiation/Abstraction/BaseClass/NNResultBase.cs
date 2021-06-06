using NATNegotiation.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace NATNegotiation.Abstraction.BaseClass
{
    public class NNResultBase : UniSpyResultBase
    {
        internal new NNErrorCode ErrorCode
        {
            get => (NNErrorCode)base.ErrorCode;
            set => base.ErrorCode = value;
        }
        public NatPacketType? PacketType { get; set; }
        public NNResultBase()
        {
            ErrorCode = NNErrorCode.NoError;
        }
    }
}
