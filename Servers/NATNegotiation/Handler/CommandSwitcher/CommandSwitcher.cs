using GameSpyLib.Logging;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure;
using NatNegotiation.Handler.CommandHandler;
using System;
using System.Net;

namespace NatNegotiation.Handler.CommandHandler.CommandSwitcher
{
    public class CommandSwitcher
    {
        public static void Switch(NatNegServer server,EndPoint endPoint, byte[] recv)
        {
            try
            {
                //check and add client into clientList
                ClientInfo client = NatNegServer.ClientList.GetOrAdd(endPoint, new ClientInfo(endPoint));
                client.LastPacketTime = DateTime.Now;

                //BytesRecieved[7] is nnpacket.PacketType.
                switch ((NatPacketType)recv[7])
                {
                    case NatPacketType.Init:
                       new InitHandler().Handle(server, client, recv);
                        break;

                    case NatPacketType.AddressCheck:
                       new AddressHandler().Handle(server, client, recv);
                        break;

                    case NatPacketType.NatifyRequest:
                        new NatifyHandler().Handle(server, client, recv);
                        break;

                    case NatPacketType.ConnectAck:
                        client.IsGotConnectAck = true;
                        break;

                    case NatPacketType.Report:
                        new ReportHandler().Handle(server, client, recv);
                        break;

                    case NatPacketType.ErtAck:
                     new ErtACKHandler().Handle(server, client, recv);
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
