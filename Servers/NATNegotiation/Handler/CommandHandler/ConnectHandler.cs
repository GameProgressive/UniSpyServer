using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Handler.SystemHandler;
using System;
using System.Net;

namespace NatNegotiation.Handler.CommandHandler
{
    public class ConnectHandler
    {
        public void Handle(NatNegServer server, EndPoint endPoint, byte[] recv)
        {
            //ConnectPacket connectPacket = new ConnectPacket(recv);
            //byte[] sendingBuffer = connectPacket.CreateReplyPacket();
            //server.SendAsync(server.Socket.RemoteEndPoint, sendingBuffer);
        }

        public static void SendConnectPacket(NatNegServer server, ClientInfo client, ClientInfo other)
        {
            ConnectPacket connPacket = new ConnectPacket
            {
                PacketType = (byte)NatPacketType.Connect,
                Cookie = client.Cookie,
                Finished = 0,
                RemoteIP = NNFormat.IPToByte(client.EndPoint),
                RemotePort = NNFormat.PortToByte(client.EndPoint)
            };
            byte[] buffer = connPacket.GenerateByteArray();

            server.SendAsync(client.EndPoint, buffer);
            client.SentConnectPacketTime = DateTime.Now;
            client.IsConnected = true;


            //send to other client
            if (other == null)
            { return; }

            connPacket.RemoteIP = NNFormat.IPToByte(other.EndPoint);
            connPacket.RemotePort = NNFormat.PortToByte(other.EndPoint);

            buffer = connPacket.GenerateByteArray();
            server.SendAsync(other.EndPoint, buffer);
            other.SentConnectPacketTime = DateTime.Now;

        }

        public static void SendDeadHeartBeatNotice(NatNegServer server, ClientInfo client)
        {
            ConnectPacket connPacket = new ConnectPacket
            {
                PacketType = (byte)NatPacketType.Connect,
                Cookie = client.Cookie,
                Finished = (byte)ConnectPacketFinishStatus.DeadHeartBeat,
                RemoteIP = ((IPEndPoint)client.EndPoint).Address.GetAddressBytes(),
                RemotePort =BitConverter.GetBytes(((IPEndPoint)client.EndPoint).Port),
            };
            byte[] buffer = connPacket.GenerateByteArray();
            server.SendAsync(client.EndPoint, buffer);
        }
    }
}
