using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NATNegotiation.Entity.Enumerator;
using NATNegotiation.Entity.Structure;
using System;
using System.Linq;
using System.Net;

namespace NatNegotiation.Handler.CommandHandler
{
    public class InitHandler
    {
        public void Handle(NatNegServer server, ClientInfo client, byte[] recv)
        {
            InitPacket initPacket = new InitPacket();
            initPacket.Parse(recv);
            if (initPacket.ErrorCode != NNErrorCode.NoError)
            {
                return;
            }

            client.Version = initPacket.Version;
            client.Cookie = initPacket.Cookie;
            client.ClientIndex = initPacket.ClientIndex;
            client.GotInit = true;

            recv[BasePacket.MagicData.Length + 1] = (byte)NatPacketType.InitAck;

            server.SendAsync(client.EndPoint, recv);

            if (client.GotConnectAck)
            {
                var c = NatNegServer.ClientList.Where(c => c.Cookie == client.Cookie && c.ClientIndex == (client.ClientIndex == 1 ? 0 : 1));
                if (c.Count() == 0)
                    return;
                ClientInfo other = c.First();

                ConnectPacket connPacket = new ConnectPacket
                {
                    RemoteIP = BitConverter.ToUInt32(((IPEndPoint)client.EndPoint).Address.GetAddressBytes(), 0),
                    RemotePort = (uint)((IPEndPoint)client.EndPoint).Port,
                    Finished = 0,
                };
                client.SentConnectTime = DateTime.Now;
                client.Connected = true;



            }

        }
    }
}
