using System;
using GameSpyLib.Logging;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure;
using NatNegotiation.Handler.CommandHandler;

namespace ServerBrowser.Handler.CommandSwitcher
{
    public class NatNegCommandSwitcher
    {
        public static void Switch(SBSession session, byte[] recv)
        {
            //we block this for now because we need @Cryptorx get information for us

            //if (!recv.Take(6).SequenceEqual(BasePacket.MagicData))
            //{
            //    //ignore it
            //    return;
            //}
            //check and add client into clientList
            NatNegClientInfo client =
                SBServer.ClientList.GetOrAdd(
                    session.Socket.RemoteEndPoint,
                    new NatNegClientInfo(session.Socket.RemoteEndPoint)
                    );
            client.LastPacketTime = DateTime.Now;

            //BytesRecieved[7] is nnpacket.PacketType.
            switch ((NatPacketType)recv[7])
            {
                case NatPacketType.Init:
                    new InitHandler(session, client, recv).Handle();
                    LogWriter.ToLog(NatPacketType.Init.ToString());
                    break;
                case NatPacketType.AddressCheck:
                    new AddressHandler(session, client, recv).Handle();
                    LogWriter.ToLog(NatPacketType.AddressCheck.ToString());
                    break;
                case NatPacketType.NatifyRequest:
                    new NatifyHandler(session, client, recv).Handle();
                    LogWriter.ToLog(NatPacketType.NatifyRequest.ToString());
                    break;
                case NatPacketType.ConnectAck:
                    client.IsGotConnectAck = true;
                    break;
                case NatPacketType.Report:
                    new ReportHandler(session, client, recv).Handle();
                    LogWriter.ToLog(NatPacketType.Report.ToString());
                    break;
                case NatPacketType.ErtAck:
                    new ErtACKHandler(session, client, recv).Handle();
                    LogWriter.ToLog(NatPacketType.ErtAck.ToString());
                    break;
                default:
                    LogWriter.UnknownDataRecieved(recv);
                    break;
            }
        }
    }
}
