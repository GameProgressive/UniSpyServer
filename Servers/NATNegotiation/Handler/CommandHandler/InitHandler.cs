using GameSpyLib.Encryption;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NATNegotiation.Entity.Enumerator;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Handler;
using NATNegotiation.Handler.SystemHandler;
using System;
using System.Linq;
using System.Net;

namespace NatNegotiation.Handler.CommandHandler
{
    public class InitHandler:NatNegHandlerBase
    {
    
        protected override void ConvertRequest(ClientInfo client, byte[] recv)
        {
            _initPacket = new InitPacket();
            _initPacket.Parse(recv);
        }

        protected override void ProcessInformation(ClientInfo client, byte[] recv)
        {
            client.Version = _initPacket.Version;
            client.PortType = _initPacket.PortType;
            Array.Copy(_initPacket.Cookie, client.Cookie, 4);
            Array.Copy(NNFormat.IPToByte(client.EndPoint), client.PublicIP, 4);
            Array.Copy(NNFormat.PortToByte(client.EndPoint), client.PublicPort, 2);
           // client.GameName = ByteExtensions.SubBytes(recv, InitPacket.Size - 1, recv.Length - 1);
            client.ClientIndex = _initPacket.ClientIndex;
            client.IsGotInit = true;
        }

        protected override void ConstructResponsePacket(ClientInfo client, byte[] recv)
        {
            //we parse everything we got about client2 into response
            var other = NatNegServer.ClientList.Where(o => o.PublicIP == _initPacket.LocalIP && o.PublicPort == _initPacket.LocalPort);
            if (other.Count() < 1)
                return;

            ClientInfo client2 = other.First();

            _initPacket.PacketType = (byte)NatPacketType.InitAck;
            _initPacket.PortType = client2.PortType;
            Array.Copy(_initPacket.LocalIP, client2.PublicIP, 4);
            Array.Copy(_initPacket.LocalPort, client2.PublicPort, 2);

            _sendingBuffer = _initPacket.GenerateByteArray();
        }
    }
}
