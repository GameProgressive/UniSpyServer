using System;
using System.Net;
using GameSpyLib.Network;
using GameSpyLib.Database;
using GameSpyLib.Logging;
using RetroSpyServer.Servers.NatNeg.Structures;
namespace RetroSpyServer.Servers.NatNeg
{
    public class NatNegServer : UdpServer
    {
        private bool replied = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbdriver">If choose sqlite for database you should pass the connection to server
        /// ,maybe NatNeg server dose not need connected to database.</param>
        /// <param name="bindTo"></param>
        /// <param name="MaxConnections"></param>
        public NatNegServer(IPEndPoint bindTo, int MaxConnections) : base(bindTo, MaxConnections)
        {
            StartAcceptAsync();
        }
        protected override void OnException(Exception e) => LogWriter.Log.WriteException(e);


        protected override void ProcessAccept(UdpPacket packet)
        {
            // If we dont reply, we must manually release the EventArgs back to the pool
            replied = false;
            IPEndPoint remote = (IPEndPoint)packet.AsyncEventArgs.RemoteEndPoint;

            // Need at least 5 bytes
            if (packet.BytesRecieved.Length < 5)
            {
                Release(packet.AsyncEventArgs);
                return;
            }
            try
            {

                switch (packet.BytesRecieved[0])
                {

                    case NatNegInfo.NN_PREINIT:
                        NatNegHelper.PreInitPacketResponse(this, packet);
                        break;
                    case NatNegInfo.NN_INIT:
                        NatNegHelper.InitPacketResponse(this, packet);
                        break;
                    case NatNegInfo.NN_ADDRESS_CHECK:
                        NatNegHelper.AddressCheckResponse(this, packet);
                        break;
                    case NatNegInfo.NN_NATIFY_REQUEST:
                        NatNegHelper.NatifyResponse(this, packet);
                        break;
                    case NatNegInfo.NN_CONNECT_PING:
                        break;
                    case NatNegInfo.NN_BACKUP_ACK:
                        break;
                    case NatNegInfo.NN_REPORT:
                        NatNegHelper.ReportResponse(this, packet);
                        break;
                    default:
                        LogWriter.Log.Write("Received unknown packet type: " + packet.BytesRecieved[0], LogLevel.Error);
                        break;


                }
            }
            catch
            { }




        }
    }

}
