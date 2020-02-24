using QueryReport.Server;
using System;
using System.Net;

namespace QueryReport.Handler.CommandHandler.ClientMessageACK
{
    public class MessageACKHandler : QRHandlerBase
    {
        protected MessageACKHandler(QRServer server, EndPoint endPoint, byte[] recv) : base(server, endPoint, recv)
        {
            throw new NotImplementedException();
        }
    }
}
