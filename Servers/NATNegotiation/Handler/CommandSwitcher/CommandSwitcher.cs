using GameSpyLib.Logging;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using NATNegotiation.Entity.Structure;
using NATNegotiation.Handler.CommandHandler;
using NATNegotiation.Handler.SystemHandler;
using System;
using System.Linq;
using System.Net;

namespace NatNegotiation.Handler.CommandHandler.CommandSwitcher
{
    public class CommandSwitcher
    {
        public static void Switch(NatNegServer server, EndPoint endPoint, byte[] recv)
        {
            ClientInfo client = NatNegServer.ClientList.Where(c => c.EndPoint == endPoint).First();
            //parse basic info
            client.Parse(recv);

            client.LastPacketTime = DateTime.Now;
            try
            {
                //BytesRecieved[7] is nnpacket.PacketType.
                switch ((NatPacketType)recv[7])
                {
                    case NatPacketType.Init:
                        InitHandler init = new InitHandler();
                        init.Handle(server,client,recv);
                        break;
                    case NatPacketType.AddressCheck:
                        AddressHandler address = new AddressHandler();
                        address.Handle(server, client, recv);
                        break;
                    case NatPacketType.NatifyRequest:
                        NatifyHandler natify = new NatifyHandler();
                        natify.Handle(server, client, recv);
                        break;
                    case NatPacketType.ConnectAck:
                        client.IsGotConnectAck = true;
                        break;
                    case NatPacketType.Report:
                        ReportHandler report = new ReportHandler();
                        report.Handle(server, client, recv);
                      break;
                    case NatPacketType.ErtAck:
                        ErtACKHandler ert = new ErtACKHandler();
                        ert.Handle(server, client, recv);
                        break;
                    default:
                        server.UnknownDataRecived(recv);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.Log.WriteException(e);
            }
        }
    }
}
