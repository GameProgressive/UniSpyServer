using System;
using System.Linq;
using GameSpyLib.Logging;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure;
using NatNegotiation.Entity.Structure.Packet;

namespace ServerBrowser.Handler.CommandHandler.NatNeg.CommandSwitcher
{
    public class NatNegCommandSwitcher
    {
        public static void Switch(SBSession session, byte[] recv)
        {
            if (!recv.Take(6).SequenceEqual(BasePacket.MagicData))
            {
                //ignore it
                return;
            }
            //check and add client into clientList
            ClientInfo client =
                SBServer.ClientList.GetOrAdd(
                    session.Socket.RemoteEndPoint,
                    new ClientInfo(session.Socket.RemoteEndPoint)
                    );

            client.LastPacketTime = DateTime.Now;

            //BytesRecieved[7] is nnpacket.PacketType.
            switch ((NatPacketType)recv[7])
            {
                case NatPacketType.Init:
                    //new InitHandler().Handle(server, client, recv);
                    break;

                case NatPacketType.AddressCheck:
                    //new AddressHandler().Handle(server, client, recv);
                    break;

                case NatPacketType.NatifyRequest:
                    // new NatifyHandler().Handle(server, client, recv);
                    break;

                case NatPacketType.ConnectAck:
                    client.IsGotConnectAck = true;
                    break;

                case NatPacketType.Report:
                    //new ReportHandler().Handle(server, client, recv);
                    break;

                case NatPacketType.ErtAck:
                    // new ErtACKHandler().Handle(server, client, recv);
                    break;

                default:
                    LogWriter.UnknownDataRecieved(recv);
                    break;
            }
        }
    }
}
