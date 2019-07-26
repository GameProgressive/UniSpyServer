using System;
using System.Net;
using GameSpyLib.Network;
using GameSpyLib.Database;
using GameSpyLib.Logging;
using RetroSpyServer.Servers.NatNeg.Structures;
using System.Net.Sockets;
using System.Collections.Generic;

namespace RetroSpyServer.Servers.NatNeg
{
    public class NatNegServer : UdpServer
    {

        public bool Replied = false;

        public NatNegPacket NNPacket;

        public List<ClientInfo> ClientInfoList = new List<ClientInfo>();

        public int InstanceCount=1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbdriver">If choose sqlite for database you should pass the connection to server
        /// ,maybe NatNeg server dose not need connected to database.</param>
        /// <param name="bindTo"></param>
        /// <param name="MaxConnections"></param>
        public NatNegServer(string serverName, IPEndPoint bindTo, int MaxConnections) : base(serverName, bindTo, MaxConnections)
        {
            StartAcceptAsync();
        }
       


        protected override void ProcessAccept(UdpPacket packet)
        {
            IPEndPoint remote = (IPEndPoint)packet.AsyncEventArgs.RemoteEndPoint;

            //copy data in udp packet to natnegpacket format prepare for reply data;
            NatNegPacket nnpacket = new NatNegPacket();

            if (!nnpacket.Set(packet.BytesRecieved))
                return;

            try
            {
                //BytesRecieved[7] is nnpacket.PacketType.
                switch (nnpacket.Common.PacketType)
                {
                    case Enumerators.NatPacketType.PreInit:
                         NatNegHelper.PreInitResponse(this, packet, nnpacket);
                         break;
                    case Enumerators.NatPacketType.Init:
                        NatNegHelper.InitPacketResponse(this, packet,nnpacket);
                        break;
                    case Enumerators.NatPacketType.AddressCheck:
                        NatNegHelper.AddressCheckResponse(this, packet, nnpacket);
                        break;
                    case Enumerators.NatPacketType.NatifyRequest:
                        NatNegHelper.NatifyResponse(this, packet, nnpacket);
                        break;
                    case Enumerators.NatPacketType.ConnectAck:
                        NatNegHelper.NNConnectResponse(this, packet, nnpacket);
                        break;
                    case Enumerators.NatPacketType.Report:
                        NatNegHelper.ReportResponse(this, packet, nnpacket);
                        break;
                    default:                       
                        LogWriter.Log.Write("{0,-8} [Recv] unknow data", LogLevel.Error, ServerName);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.Log.WriteException(e);
            }
            finally
            {
                if (Replied == true)
                    Release(packet.AsyncEventArgs);
            }
        }

        protected override void OnException(Exception e) => LogWriter.Log.WriteException(e);

    }

}
