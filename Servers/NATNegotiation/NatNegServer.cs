using GameSpyLib.Logging;
using GameSpyLib.Network;
using GameSpyLib.Network.UDP;
using NATNegotiation.Enumerator;
using NATNegotiation.Handler;
using NATNegotiation.Structure;
using NATNegotiation.Structure.Packet;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace NATNegotiation
{
    public class NatNegServer : UDPServer
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
        public NatNegServer(string serverName, IPEndPoint bindTo, int MaxConnections) : base(serverName, bindTo, MaxConnections)
        {
            StartAcceptAsync();
        }



        protected override void ProcessAccept(UDPPacket packet)
        {
            IPEndPoint remote = (IPEndPoint)packet.AsyncEventArgs.RemoteEndPoint;


            //Task.Run(() =>
            //{

            if (packet.BytesRecieved.Length < 6)
                return;
            BasePacket basePacket = new BasePacket(packet.BytesRecieved);
            try
            {
                //BytesRecieved[7] is nnpacket.PacketType.
                switch (basePacket.PacketType)
                {
                    case NatPacketType.PreInit:
                        //NatNegHandler.PreInitResponse(this, packet, nnpacket);
                        break;
                    case NatPacketType.Init:
                        InitHandler.InitResponse(this, packet);
                        break;
                    case NatPacketType.AddressCheck:
                        AddressHandler.AddressCheckResponse(this, packet);
                        break;
                    case NatPacketType.NatifyRequest:
                        NatifyHandler.NatifyResponse(this, packet);
                        break;
                    case NatPacketType.ConnectAck:
                        ConnectHandler.ConnectResponse(this, packet);
                        break;
                    case NatPacketType.Report:
                        ReportHandler.ReportResponse(this, packet);
                        break;
                    default:
                        LogWriter.Log.Write(LogLevel.Error, "{0,-8} [Recv] unknow data", ServerName);
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
            //});
        protected override void OnException(Exception e) => LogWriter.Log.WriteException(e);

    }

}
