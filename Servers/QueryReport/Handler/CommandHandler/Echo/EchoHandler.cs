using System;
using System.Net;
using QueryReport.Server;

namespace QueryReport.Handler.CommandHandler.Echo
{
    public class EchoHandler : QRHandlerBase
    {
        public EchoHandler(QRServer server, EndPoint endPoint, byte[] recv) : base(server, endPoint, recv)
        {
        }

        protected override void DataOperation(QRServer server, EndPoint endPoint, byte[] recv)
        {
            //TODO
            //add recive echo packet on gameserverList
        }
    }
}
