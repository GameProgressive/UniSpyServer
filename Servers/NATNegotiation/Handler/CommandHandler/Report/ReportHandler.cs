using GameSpyLib.Logging;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NatNegotiation.Entity.Structure;
using NatNegotiation.Handler.CommandHandler;
using System.Net;
using GameSpyLib.Common.Entity.Interface;

namespace NatNegotiation.Handler.CommandHandler
{
    /// <summary>
    /// Get nat neg result report success or fail
    /// </summary>
    public class ReportHandler : CommandHandlerBase
    {
        public ReportHandler(IClient client, NatNegClientInfo clientInfo, byte[] recv) : base(client, clientInfo, recv)
        {
        }

        protected override void CheckRequest()
        {
            _reportPacket = new ReportPacket();
            _reportPacket.Parse(_recv);
        }

        protected override void DataOperation()
        {
            _clientInfo.IsGotReport = true;
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = _reportPacket.GenerateResponse(NatPacketType.ReportAck);
        }

        protected override void Response()
        {
            LogWriter.ToLog("Client: " + ((IPEndPoint)_clientInfo.RemoteEndPoint).Address.ToString() + "natneg failed!");
            base.Response();
        }
    }
}
