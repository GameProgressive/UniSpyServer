using UniSpyServer.Servers.QueryReport.V2.Abstraction.BaseClass;

namespace UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Result
{
    public sealed class ClientMessageResult : ResultBase
    {
        public byte[] NatNegMessage { get; set; }
        public int? MessageKey { get; set; }
        public ClientMessageResult()
        {
            //we need to change packet type to client message then send
            PacketType = Enumerate.PacketType.ClientMessage;
        }
    }
}
