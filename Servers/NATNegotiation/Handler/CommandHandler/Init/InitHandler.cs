using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Handler.CommandHandler;

namespace NatNegotiation.Handler.CommandHandler
{
    public class InitHandler : CommandHandlerBase
    {

        protected override void ConvertRequest(ClientInfo client, byte[] recv)
        {
            _initPacket = new InitPacket();
            _initPacket.Parse(recv);
        }

        protected override void ProcessInformation(ClientInfo client, byte[] recv)
        {
            client.Parse(_initPacket);
            // client.GameName = ByteExtensions.SubBytes(recv, InitPacket.Size - 1, recv.Length - 1);
        }

        protected override void ConstructResponsePacket(ClientInfo client, byte[] recv)
        {
            //we parse everything we got about client2 into response
            //var other = NatNegServer.ClientList.Where(o => o.PublicIP == _initPacket.LocalIP && o.PublicPort == _initPacket.LocalPort);
            //if (other.Count() < 1)
            //    return;

            //client.TargetClient = other.First();
            //other.First().TargetClient = client;

            //ClientInfo client2 = other.First();

            _initPacket.PacketType = (byte)NatPacketType.InitAck;
            //Array.Copy(client2.PublicIP, _initPacket.LocalIP, 4);
            //Array.Copy(client2.PublicPort, _initPacket.LocalPort, 2);
            _sendingBuffer = _initPacket.GenerateByteArray();

            //_connPacket = new ConnectPacket();
            //_connPacket.Version = client.Version;
            //_connPacket.PacketType = (byte)NatPacketType.Connect;
            //Array.Copy(client.Cookie, _connPacket.Cookie, 4);
            //Array.Copy(_connPacket.RemoteIP, client.PublicIP, 4);
            //Array.Copy(_connPacket.RemotePort, client.PublicPort, 4);
        }

        protected override void SendResponse(NatNegServer server, ClientInfo client)
        {

            base.SendResponse(server, client);
        }
    }
}
