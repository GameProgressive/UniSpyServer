using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Handler;

namespace NatNegotiation.Handler.CommandHandler
{
    public class ReportHandler : NatNegHandlerBase
    {

        protected override void ConvertRequest(ClientInfo client, byte[] recv)
        {
            _reportPacket = new ReportPacket();
            _reportPacket.Parse(recv);
        }

        protected override void ProcessInformation(ClientInfo client, byte[] recv)
        {
            client.IsGotReport = true;
        }
        protected override void ConstructResponsePacket(ClientInfo client, byte[] recv)
        {
            _reportPacket.PacketType = (byte)NatPacketType.ReportAck;
            _sendingBuffer = _reportPacket.GenerateByteArray();
        }
    }
}
