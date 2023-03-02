using UniSpy.Server.QueryReport.Abstraction.BaseClass;

namespace UniSpy.Server.QueryReport.Contract.Result
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
