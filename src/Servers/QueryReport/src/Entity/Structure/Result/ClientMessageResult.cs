using UniSpyServer.QueryReport.Abstraction.BaseClass;
using UniSpyServer.QueryReport.Entity.Enumerate;

namespace UniSpyServer.QueryReport.Entity.Structure.Result
{
    public sealed class ClientMessageResult : ResultBase
    {
        public uint InstantKey { get; set; }
        public byte[] NatNegMessage { get; set; }
        public int? MessageKey { get; set; }
        public ClientMessageResult()
        {
            //we need to change packet type to client message then send
            PacketType = Enumerate.PacketType.ClientMessage;
        }
    }
}
