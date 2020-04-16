using System;
using GameSpyLib.Logging;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure;
using NatNegotiation.Handler.CommandHandler;

namespace ServerBrowser.Handler.CommandSwitcher
{
    public class NatNegCommandSwitcher
    {
        public static void Switch(SBSession client, byte[] recv)
        {
            //check and add client into clientList
            NatNegClientInfo clientInfo =
                SBServer.ClientList.GetOrAdd(
                    client.Socket.RemoteEndPoint,
                    new NatNegClientInfo(client.Socket.RemoteEndPoint)
                    );
            clientInfo.LastPacketTime = DateTime.Now;

            //BytesRecieved[7] is nnpacket.PacketType.
            switch ((NatPacketType)recv[7])
            {
                case NatPacketType.Init:
                    new InitHandler(client, clientInfo, recv).Handle();
                    LogWriter.ToLog(NatPacketType.Init.ToString());
                    break;
                case NatPacketType.AddressCheck:
                    new AddressHandler(client, clientInfo, recv).Handle();
                    LogWriter.ToLog(NatPacketType.AddressCheck.ToString());
                    break;
                case NatPacketType.NatifyRequest:
                    new NatifyHandler(client, clientInfo, recv).Handle();
                    LogWriter.ToLog(NatPacketType.NatifyRequest.ToString());
                    break;
                case NatPacketType.ConnectAck:
                    clientInfo.IsGotConnectAck = true;
                    break;
                case NatPacketType.Report:
                    new ReportHandler(client, clientInfo, recv).Handle();
                    LogWriter.ToLog(NatPacketType.Report.ToString());
                    break;
                case NatPacketType.ErtAck:
                    new ErtACKHandler(client, clientInfo, recv).Handle();
                    LogWriter.ToLog(NatPacketType.ErtAck.ToString());
                    break;
                default:
                    LogWriter.UnknownDataRecieved(recv);
                    break;
            }
        }
    }
}
