using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;

namespace QueryReport.Entity.Structure.Result
{
    internal sealed class ClientMessageResult : QRResultBase
    {
        public int InstantKey { get; set; }
        public byte[] NatNegMessage { get; set; }
        public int? MessageKey { get; set; }
        public ClientMessageResult()
        {
            //we need to change packet type to client message then send
            PacketType = QRPacketType.ClientMessage;
        }
    }
}
