using System;
using System.Net;
using GameSpyLib.Network;
using GameSpyLib.Database;
using GameSpyLib.Logging;
using RetroSpyServer.Servers.NatNeg.Structures;
using System.Net.Sockets;

namespace RetroSpyServer.Servers.NatNeg
{
    public class NatNegServer : UdpServer
    {

        public bool _replied = false;
        
        public NatNegPacket _nnpacket;
        public uint _cookie;
        public byte _clientIndex;
        public byte _clientVersion;
        public byte _state;
        public uint _clientID;
        public bool _foundPartner;
        public string _gamename;
        public bool _got_natify_request;
        public bool _got_preinit;
        public bool _sent_connect;



        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbdriver">If choose sqlite for database you should pass the connection to server
        /// ,maybe NatNeg server dose not need connected to database.</param>
        /// <param name="bindTo"></param>
        /// <param name="MaxConnections"></param>
        public NatNegServer(string serverName,IPEndPoint bindTo, int MaxConnections) : base(serverName, bindTo, MaxConnections)
        {            
            StartAcceptAsync();
        }
        protected override void OnException(Exception e) => LogWriter.Log.WriteException(e);


        protected override void ProcessAccept(UdpPacket upacket)
        {
            
            IPEndPoint remote = (IPEndPoint)upacket.AsyncEventArgs.RemoteEndPoint;

            // Need at least 5 bytes
            if (upacket.BytesRecieved.Length < 5)
            {
                Release(upacket.AsyncEventArgs);
                return;
            }

            //check if udp data is NatNeg format
            if (NatNegHelper.IsNetNegData(upacket) == false)
                return;
            //save what client send to natnegpacket
            NatNegHelper.SaveNatNegPacket(this,upacket);

            int packetSize = NatNegHelper.packetSizeFromType(_nnpacket.packettype);

            try
            {

                switch (upacket.BytesRecieved[7])
                {

                    case NNRequest.NN_PREINIT:
                        NatNegHelper.PreInitPacketResponse(this, upacket);
                        break;
                    case NNRequest.NN_INIT:
                        NatNegHelper.InitPacketResponse(this, upacket);
                        break;
                    case NNRequest.NN_ADDRESS_CHECK:
                        NatNegHelper.AddressCheckResponse(this, upacket);
                        break;
                    case NNRequest.NN_NATIFY_REQUEST:
                        NatNegHelper.NatifyResponse(this,upacket);
                        break;
                    case NNRequest.NN_CONNECT_PING:
                        break;
                    case NNRequest.NN_BACKUP_ACK:
                        break;
                    case NNRequest.NN_REPORT:
                        NatNegHelper.ReportResponse(this, upacket);
                        break;
                    default:
                        //LogWriter.Log.Write("Received unknown packet type: " + BitConverter.ToString(packet.BytesRecieved), LogLevel.Error);
                        //LogWriter.Log.Write("[NatNeg] received unknow data: " + Convert.ToString(packet.BytesRecieved[0],16), LogLevel.Error);
                        LogWriter.Log.Write("{0,-8} [Recv] unknow data" , LogLevel.Error,ServerName);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.Log.WriteException(e);
            }
            finally
            {
                if (_replied == true)
                    Release(upacket.AsyncEventArgs);
            }
        }
    }
}
