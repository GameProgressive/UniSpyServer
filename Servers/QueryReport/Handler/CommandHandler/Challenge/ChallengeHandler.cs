using QueryReport.Server;
using System.Net;

namespace QueryReport.Handler.CommandHandler.Challenge
{
    public class ChallengeHandler : QRHandlerBase
    {
        public ChallengeHandler(QRServer server, EndPoint endPoint, byte[] recv) : base(server, endPoint, recv)
        {
        }

        protected override void CheckRequest(QRServer server, EndPoint endPoint, byte[] recv)
        {
            //TODO
            base.CheckRequest(server, endPoint, recv);
        }

        protected override void DataOperation(QRServer server, EndPoint endPoint, byte[] recv)
        {
            //TODO
            base.DataOperation(server, endPoint, recv);
        }
    }
}
