using GameSpyLib.Logging;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NatNegotiation.Entity.Structure;
using NatNegotiation.Handler.CommandHandler;
using System.Net;

namespace NatNegotiation.Handler.CommandHandler
{
    /// <summary>
    /// Get nat neg result report success or fail
    /// </summary>
    public class ReportHandler : CommandHandlerBase
    {
        protected override void CheckRequest(ClientInfo client, byte[] recv)
        {
            _reportPacket = new ReportPacket();
            _reportPacket.Parse(recv);
        }

        protected override void DataOperation(ClientInfo client, byte[] recv)
        {
            client.IsGotReport = true;
        }

        protected override void ConstructResponse(ClientInfo client, byte[] recv)
        {
            _sendingBuffer = _reportPacket.GenerateResponse(NatPacketType.ReportAck);
        }

        protected override void  Response(NatNegServer server, ClientInfo client)
        {
            LogWriter.ToLog("Client: " + ((IPEndPoint)client.RemoteEndPoint).Address.ToString() + "natneg failed!");
            base.Response(server, client);
        }
    }
}
