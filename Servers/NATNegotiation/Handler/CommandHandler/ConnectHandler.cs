using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Handler;
using NATNegotiation.Handler.SystemHandler;
using System;
using System.Linq;
using System.Net;

namespace NatNegotiation.Handler.CommandHandler
{
    public class ConnectHandler : NatNegHandlerBase
    { 
        public static void SendConnectPacket(NatNegServer server, ClientInfo client, ClientInfo other)
        {
            ConnectPacket connPacket = new ConnectPacket
            {
                PacketType = (byte)NatPacketType.Connect,
                Cookie = client.Cookie,
                Finished = 0,
                RemoteIP = client.PublicIP,
                RemotePort = client.PublicPort,
            };

            byte[] buffer = connPacket.GenerateByteArray();

            server.SendAsync(client.RemoteEndPoint, buffer);
            client.SentConnectPacketTime = DateTime.Now;
            client.IsConnected = true;


            //send to other client
            if (other == null)
            { return; }

            connPacket.RemoteIP = NNFormat.IPToByte(other.RemoteEndPoint);
            connPacket.RemotePort = NNFormat.PortToByte(other.RemoteEndPoint);

            buffer = connPacket.GenerateByteArray();
            server.SendAsync(other.RemoteEndPoint, buffer);
            other.SentConnectPacketTime = DateTime.Now;

        }

        public static void SendDeadHeartBeatNotice(NatNegServer server, ClientInfo client)
        {
            ConnectPacket connPacket = new ConnectPacket
            {
                PacketType = (byte)NatPacketType.Connect,
                Cookie = client.Cookie,
                Finished = (byte)ConnectPacketFinishStatus.DeadHeartBeat,
                RemoteIP = ((IPEndPoint)client.RemoteEndPoint).Address.GetAddressBytes(),
                RemotePort = BitConverter.GetBytes(((IPEndPoint)client.RemoteEndPoint).Port),
            };
            byte[] buffer = connPacket.GenerateByteArray();
            server.SendAsync(client.RemoteEndPoint, buffer);
        }

        protected override void ConvertRequest(ClientInfo client, byte[] recv)
        {
            _connPacket = new ConnectPacket();
            _connPacket.Parse(recv);
        }

        protected override void ProcessInformation(ClientInfo client, byte[] recv)
        {



        }

        protected override void ConstructResponsePacket(ClientInfo client, byte[] recv)
        {

        }
        protected override void SendResponse(NatNegServer server, ClientInfo client)
        {
            var other = NatNegServer.ClientList.Values.Where(
                c => c.PublicIP.SequenceEqual(_connPacket.RemoteIP)
                && c.PublicPort.SequenceEqual(_connPacket.RemotePort)
                );
            if (other.Count() < 1)
            {
                server.ToLog("Can not find client2 that client1 tries to connect!");
                return;
            }
            ClientInfo client2 = other.First();
            _sendingBuffer = _connPacket.GenerateByteArray();
            server.SendAsync(client2.RemoteEndPoint, _sendingBuffer);
        }
    }
}
