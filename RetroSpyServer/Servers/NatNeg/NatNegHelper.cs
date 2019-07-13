using System;
using System.Text;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using RetroSpyServer.Servers.NatNeg.Structures;

namespace RetroSpyServer.Servers.NatNeg
{
    class NatNegHelper
    {
        public static void PreInitPacketResponse(NatNegServer server, UdpPacket packet)
        {
            //LogWriter.Log.Write("[NATNEG] No impliment function for PreInitPacket!", LogLevel.Debug);
            //TODO
            //server._sent_connect = false;
            //server._clientID = server._nnpacket.packet.Preinit.clientID;
            //server._clientIndex = server._nnpacket.packet.Preinit.clientIndex;
            //server._got_preinit = true;
            //SetPreInitPacket(NatNegInfo.NN_PREINIT_READY);
           
            server.ReplyAsync(packet,packet.BytesRecieved);
        }

        private static byte[] SetPreInitPacket(int nN_PREINIT_READY)
        {
            byte[] hello = { 0x08, 0x09 };
            return hello;
        }

        public static void InitPacketResponse(NatNegServer server, UdpPacket packet)
        {
            //LogWriter.Log.Write("[NATNEG] No impliment function for InitPacket!", LogLevel.Debug);
            server.ReplyAsync(packet, packet.BytesRecieved);
        }

        public static void AddressCheckResponse(NatNegServer server, UdpPacket packet)
        {
            //LogWriter.Log.Write("[NATNEG] No impliment function for AddressCheckPacket!", LogLevel.Debug);
            server.ReplyAsync(packet, packet.BytesRecieved);
        }

        public static void NatifyResponse(NatNegServer server, UdpPacket packet)
        {
            server.ReplyAsync(packet, packet.BytesRecieved);
        }

        public static void ReportResponse(NatNegServer server, UdpPacket packet)
        {
            server.ReplyAsync(packet, packet.BytesRecieved);
        }

        public static void SaveNatNegPacket(NatNegServer server, UdpPacket upacket)
        {
            
            server._nnpacket.version =upacket.BytesRecieved[6];
            server._nnpacket.packettype= upacket.BytesRecieved[7];


        }

        /// <summary>
        /// Check the incoming udp data is a NatNeg format data
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        public static bool IsNetNegData(UdpPacket packet)
        {
            if (packet.BytesRecieved[0] != NNMagicData.NN_MAGIC_0)
                return false;
            if (packet.BytesRecieved[1] != NNMagicData.NN_MAGIC_1)
                return false;
            if (packet.BytesRecieved[2] != NNMagicData.NN_MAGIC_2)
                return false;
            if (packet.BytesRecieved[3] != NNMagicData.NN_MAGIC_3)
                return false;
            if (packet.BytesRecieved[4] != NNMagicData.NN_MAGIC_4)
                return false;
            if (packet.BytesRecieved[5] != NNMagicData.NN_MAGIC_5)
                return false;

            return true;
        }

        public static int packetSizeFromType(byte type)
        {
            int size = 0;
            switch (type)
            {
                case NNRequest.NN_PREINIT:

                case NNRequest.NN_PREINIT_ACK:
                    size = NatNegInfo.PREINITPACKET_SIZE;
                    break;
                case NNRequest.NN_ADDRESS_REPLY:
                case NNRequest.NN_NATIFY_REQUEST:
                case NNRequest.NN_ERTTEST:
                case NNRequest.NN_INIT:
                case NNRequest.NN_INITACK:
                    size = NatNegInfo.INITPACKET_SIZE;
                    break;
                case NNRequest.NN_CONNECT_ACK:
                case NNRequest.NN_CONNECT_PING:
                case NNRequest.NN_CONNECT:
                    size = NatNegInfo.CONNECTPACKET_SIZE;
                    break;
                case NNRequest.NN_REPORT:
                case NNRequest.NN_REPORT_ACK:
                    size = NatNegInfo.REPORTPACKET_SIZE;
                    break;

            }
            return size;
        }
    }
}
