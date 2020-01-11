using GameSpyLib.Logging;
using NATNegotiation.Entity.Enumerator;
using NATNegotiation.Entity.Structure.Packet;
using System;
using System.Collections.Generic;
using System.Text;

namespace NATNegotiation.Handler.CommandHandler.CommandSwitcher
{
    public class CommandSwitcher
    {
        public static void Switch(NatNegServer server,byte[] message)
        {
            BasePacket basePacket = new BasePacket(message);
            try
            {
                //BytesRecieved[7] is nnpacket.PacketType.
                switch (basePacket.PacketType)
                {
                    case NatPacketType.PreInit:
                        //NatNegHandler.PreInitResponse(this, packet, nnpacket);
                        break;
                    case NatPacketType.Init:
                        InitHandler.InitResponse(server, message);
                        break;
                    case NatPacketType.AddressCheck:
                        AddressHandler.AddressCheckResponse(server, message);
                        break;
                    case NatPacketType.NatifyRequest:
                        NatifyHandler.NatifyResponse(server, message);
                        break;
                    case NatPacketType.ConnectAck:
                        ConnectHandler.ConnectResponse(server, message);
                        break;
                    case NatPacketType.Report:
                        ReportHandler.ReportResponse(server, message);
                        break;
                    default:
                        server.UnknownDataRecived(message);
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
