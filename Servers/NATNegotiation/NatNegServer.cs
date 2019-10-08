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
    public class NatNegServer : UdpServer
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
        public NatNegServer(string serverName, DatabaseDriver databaseDriver,IPEndPoint bindTo, int MaxConnections) : base(serverName, bindTo, MaxConnections)
        {
            Start();
        }
        protected override void OnStarted()
        {
            // Start receive datagrams
            ReceiveAsync();
        }

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            Console.WriteLine("Incoming: " + Encoding.UTF8.GetString(buffer, (int)offset, (int)size));

            if (size < 6 && size > 2048)
            { return; }
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
        }

        protected override void OnSent(EndPoint endpoint, long sent)
        {
            // Continue receive datagrams
            ReceiveAsync();
        }

        protected override void OnError(SocketError error)
        {
            string errorMsg = Enum.GetName(typeof(SocketError), error);
            LogWriter.Log.Write(errorMsg, LogLevel.Error);
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

    }

}
