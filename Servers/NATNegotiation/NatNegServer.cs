using GameSpyLib.Database;
using GameSpyLib.Logging;
using NATNegotiation.Enumerator;
using NATNegotiation.Handler;
using NATNegotiation.Structure;
using System;
using System.Collections.Generic;
using System.Net;
using GameSpyLib.Network;
using System.Net.Sockets;
using System.Text;

namespace NATNegotiation
{
    public class NatNegServer : TemplateUdpServer
    {

        public bool Replied = false;

        public List<ClientInfo> ClientInfoList = new List<ClientInfo>();

        public int InstanceCount = 1;

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

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            if (size < 6 && size > 2048)
            {
                return;
            }

            base.OnReceived(endpoint, buffer, offset, size);

            byte[] message = new byte[(int)size];
            Array.Copy(buffer, 0, message, 0, (int)size);

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
