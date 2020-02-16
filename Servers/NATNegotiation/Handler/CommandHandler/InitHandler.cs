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
            Array.Copy(initPacket.Cookie, client.Cookie,4);
            client.ClientIndex = initPacket.ClientIndex;
            client.IsGotInit = true;

            recv[BasePacket.MagicData.Length + 1] = (byte)NatPacketType.InitAck;

            server.SendAsync(client.EndPoint, recv);

            if (client.IsGotConnectAck)
            {
                var c = NatNegServer.ClientList.Where(c => c.Cookie == client.Cookie && c.ClientIndex == (client.ClientIndex == 1 ? 0 : 1) && c != client);

                if (c.Count() == 0)
                    return;
                ClientInfo other = c.First();

                if (client.IsGotConnectAck || other.IsGotConnectAck || !client.IsGotInit)
                    return;

                ConnectHandler.SendConnectPacket(server, client, other);

            }
        }
    }
}
