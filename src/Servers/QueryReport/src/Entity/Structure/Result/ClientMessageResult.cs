using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.Entity.Enumerate;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Result
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
