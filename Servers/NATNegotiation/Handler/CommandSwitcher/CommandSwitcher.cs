using GameSpyLib.Logging;
using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NatNegotiation.Handler.CommandHandler.CommandSwitcher
{
    public class CommandSwitcher
    {
        public static void Switch(NatNegServer server, EndPoint endPoint, byte[] message)
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
                        InitHandler init = new InitHandler(server,endPoint, message);
                        break;
                    case NatPacketType.AddressCheck:
                        AddressHandler.AddressCheckHandler(server,endPoint, message);
                        break;
                    case NatPacketType.NatifyRequest:
                        NatifyHandler.NatifyHandler(server,endPoint, message);
                        break;
                    case NatPacketType.ConnectAck:
                        ConnectHandler.ConnectHandler(server,endPoint, message);
                        break;
                    case NatPacketType.Report:
                        ReportHandler.ReportHandler(server,endPoint, message);
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
