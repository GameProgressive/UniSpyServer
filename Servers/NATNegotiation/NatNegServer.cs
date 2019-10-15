using GameSpyLib.Database;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using NATNegotiation.Enumerator;
using NATNegotiation.Handler;
using NATNegotiation.Structure;
using System;
using System.Net;

namespace NATNegotiation
{
    public class NatNegServer : TemplateUdpServer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbdriver">If choose sqlite for database you should pass the connection to server
        /// ,maybe NatNeg server dose not need connected to database.</param>
        /// <param name="bindTo"></param>
        /// <param name="MaxConnections"></param>
        public NatNegServer(string serverName, DatabaseDriver databaseDriver, IPAddress address, int port) : base(serverName, address, port)
        {

        }

        protected override void OnReceived(EndPoint endpoint, byte[] message)
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
                        InitHandler.InitResponse(this, endpoint, message);
                        break;
                    case NatPacketType.AddressCheck:
                        AddressHandler.AddressCheckResponse(this, endpoint, message);
                        break;
                    case NatPacketType.NatifyRequest:
                        NatifyHandler.NatifyResponse(this, endpoint, message);
                        break;
                    case NatPacketType.ConnectAck:
                        ConnectHandler.ConnectResponse(this, endpoint, message);
                        break;
                    case NatPacketType.Report:
                        ReportHandler.ReportResponse(this, endpoint, message);
                        break;
                    default:
                        UnknownDataRecived(message);
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
