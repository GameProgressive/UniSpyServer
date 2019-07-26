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

            if (IsCorrectNetNegPacket(packet) == false)
                return;
            //copy data in udp packet to natnegpacket format prepare for reply data;
            NatNegPacket nnpacket = new NatNegPacket(packet.BytesRecieved);

            try
            {
                //BytesRecieved[7] is nnpacket.PacketType.
                switch (packet.BytesRecieved[7])
                {
                    //case NNRequest.NN_PREINIT:
                    //    NatNegHelper.PreInitResponse(this,packet,nnpacket);
                    //    break;
                    case NNRequest.NN_INIT:
                        NatNegHelper.InitPacketResponse(this, packet,nnpacket);
                        break;
                    case NNRequest.NN_ADDRESS_CHECK:
                        NatNegHelper.AddressCheckResponse(this, packet);
                        break;
                    case NNRequest.NN_NATIFY_REQUEST:
                        NatNegHelper.NatifyResponse(this, packet);
                        break;
                    case NNRequest.NN_CONNECT_PING:
                        NatNegHelper.NNConnectResponse(this, packet);
                        break;
                    case NNRequest.NN_CONNECT_ACK:
                        NatNegHelper.NNConnectResponse(this, packet);
                        break;
                    case NNRequest.NN_REPORT:
                        NatNegHelper.ReportResponse(this, packet);
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
        private bool IsCorrectNetNegPacket(UdpPacket packet)
        {
            // Need at least 5 bytes
            if (packet.BytesRecieved.Length < 5)
            {
                Release(packet.AsyncEventArgs);
                return false;
            }
            if (NatNegHelper.IsNetNegData(packet) == false)
            {
                return false;
            }

            return true;

        }


        protected override void OnException(Exception e) => LogWriter.Log.WriteException(e);

    }

}
