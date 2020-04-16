using GameSpyLib.Logging;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure;
using NatNegotiation.Server;
using System;

namespace NatNegotiation.Handler.CommandHandler.CommandSwitcher
{
    public class CommandSwitcher
    {
        public static void Switch(NatNegClient client, byte[] recv)
        {
            try
            {
                //check and add client into clientList
                NatNegClientInfo clientInfo =
                    NatNegServer.ClientList.GetOrAdd(client.RemoteEndPoint, new NatNegClientInfo(client.RemoteEndPoint));
                clientInfo.LastPacketTime = DateTime.Now;

                //BytesRecieved[7] is nnpacket.PacketType.
                switch ((NatPacketType)recv[7])
                {
                    case NatPacketType.Init:
                       new InitHandler(client, clientInfo, recv).Handle();
                        break;
                    case NatPacketType.AddressCheck:
                       new AddressHandler(client, clientInfo, recv).Handle();
                        break;
                    case NatPacketType.NatifyRequest:
                        new NatifyHandler(client, clientInfo, recv).Handle();
                        break;
                    case NatPacketType.ConnectAck:
                        clientInfo.IsGotConnectAck = true;
                        break;
                    case NatPacketType.Report:
                        new ReportHandler(client, clientInfo, recv).Handle();
                        break;
                    case NatPacketType.ErtAck:
                     new ErtACKHandler(client, clientInfo, recv).Handle();
                        break;
                    default:
                        LogWriter.UnknownDataRecieved(recv);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.ToLog(Serilog.Events.LogEventLevel.Error, e.ToString());
            }
        }
    }
}
