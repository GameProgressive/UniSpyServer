using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Handler.CommandHandler;

namespace NatNegotiation.Handler.CommandHandler
{
    /// <summary>
    /// Get nat neg result report success or fail
    /// </summary>
    public class ReportHandler : CommandHandlerBase
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
        protected override void SendResponse(NatNegServer server, ClientInfo client)
        {
            server.ToLog("Client: " + client.PublicIP + "natneg failed!");
            base.SendResponse(server, client);
        }
    }
}
