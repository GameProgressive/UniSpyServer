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
            base.CheckRequest(server, endPoint, recv);
        }

        protected override void DatabaseOperation(QRServer server, EndPoint endPoint, byte[] recv)
        {
            base.DatabaseOperation(server, endPoint, recv);
        }

        protected override void ConstructeResponse(QRServer server, EndPoint endPoint, byte[] recv)
        {
            _sendingBuffer = new byte[24];
            Array.Copy(BaseResponsePacket.MagicData, _sendingBuffer, 2);
            _sendingBuffer[2] = (byte)QRPacketType.Challenge;
            Array.Copy(recv, 0, _sendingBuffer, 3, 4);

            // Challenge
            _sendingBuffer[7] = 0x54;
            _sendingBuffer[8] = 0x54;
            _sendingBuffer[9] = 0x54;

            _sendingBuffer[10] = 0x00;
            _sendingBuffer[11] = 0x00;
            // IP
            IPEndPoint iPEndPoint = (IPEndPoint)endPoint;
            Array.Copy(iPEndPoint.Address.GetAddressBytes(), 0, _sendingBuffer, 12, 4);
            _sendingBuffer[16] = 0;
            _sendingBuffer[17] = 0;
            _sendingBuffer[18] = 0;
            _sendingBuffer[19] = 0;

            //Port
            Array.Copy(BitConverter.GetBytes(iPEndPoint.Port), 0, _sendingBuffer, 20, 4);
        }
    }
}
