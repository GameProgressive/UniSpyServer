using NATNegotiation.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace NATNegotiation.Abstraction.BaseClass
{
    public class NNResultBase : UniSpyResultBase
    {
        public new NNErrorCode ErrorCode
        {
            get { return (NNErrorCode)base.ErrorCode; }
            set { base.ErrorCode = value; }
        }
        public NatPacketType? PacketType { get; set; }
        public NNResultBase()
        {
            ErrorCode = NNErrorCode.NoError;
        }
    }
}
