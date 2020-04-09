using GameSpyLib.Logging;
using NatNegotiation.Entity.Enumerator;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Handler.CommandHandler;
using System;

namespace NatNegotiation.Handler.CommandHandler.CommandSwitcher
{
    public class CommandSwitcher
    {
        public static void Switch(NatNegServer server, ClientInfo client, byte[] recv)
        {
            try
            {
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
                        server.UnknownDataRecived(recv);
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
