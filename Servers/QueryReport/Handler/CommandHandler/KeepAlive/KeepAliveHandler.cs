using QueryReport.Entity.Structure.Packet;
using QueryReport.Server;
using System.Net;

namespace QueryReport.Handler.CommandHandler.KeepAlive
{
    public class KeepAliveHandler : QRHandlerBase
    {
        public KeepAliveHandler(QRServer server, EndPoint endPoint, byte[] recv) : base(server, endPoint, recv)
        {
        }

        protected override void ConstructeResponse(QRServer server, EndPoint endPoint, byte[] recv)
        {
            KeepAlivePacket keep = new KeepAlivePacket(recv);
            _sendingBuffer = keep.GenerateResponse();
        }
    }
}
