using QueryReport.Entity.Enumerator;
using System;
using System.Net;

namespace QueryReport.Entity.Structure.Packet
{
    public class ChallengePacket : BaseResponsePacket
    {
        public byte[] RemoteIP = new byte[4];
        public byte[] RemotePort = new byte[4];

        public ChallengePacket(EndPoint endPoint, byte[] recv) : base(recv)
        {
            PacketType = (byte)QRPacketType.Challenge;
            Array.Copy(((IPEndPoint)endPoint).Address.GetAddressBytes(), RemoteIP, 4);
            Array.Copy(BitConverter.GetBytes(((IPEndPoint)endPoint).Port), RemotePort, 4);
        }
        public override byte[] GenerateResponse()
        {
            byte[] buffer = new byte[24];
            base.GenerateResponse().CopyTo(buffer, 0);
            // Challenge
            buffer[7] = 0x54;
            buffer[8] = 0x54;
            buffer[9] = 0x54;

            buffer[10] = 0x00;
            buffer[11] = 0x00;
            // IP
            RemoteIP.CopyTo(buffer, 12);
            buffer[16] = 0;
            buffer[17] = 0;
            buffer[18] = 0;
            buffer[19] = 0;

            //Port
            RemotePort.CopyTo(buffer, 20);
            return buffer;
        }
    }
}
