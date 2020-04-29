using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure;
using NatNegotiation.Entity.Structure.Packet;
using System.Net;

namespace NatNegotiation.Handler.CommandHandler
{
    /// <summary>
    /// Get nat neg result report success or fail
    /// </summary>
    public class ReportHandler : NatNegCommandHandlerBase
    {
        public ReportHandler(ISession session, NatNegClientInfo clientInfo, byte[] recv) : base(session, clientInfo, recv)
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
