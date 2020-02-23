using QueryReport.Entity.Enumerator;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Packet;
using QueryReport.Server;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace QueryReport.Handler.CommandHandler.HeartBeat
{
    public class HeartBeatHandler:QRHandlerBase
    {
       
        public HeartBeatHandler(QRServer server, EndPoint endPoint, byte[] recv) : base(server, endPoint, recv)
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
            //Save server information.
            base.DataOperation(server, endPoint, recv);
        }

        protected override void ConstructeResponse(QRServer server, EndPoint endPoint, byte[] recv)
        {
            ChallengePacket challenge = new ChallengePacket(endPoint, recv);
            _sendingBuffer = challenge.GenerateResponse();
        }
    }
}
