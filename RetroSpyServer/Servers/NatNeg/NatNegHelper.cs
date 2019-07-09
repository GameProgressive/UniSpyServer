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
            server.ReplyAsync(packet,packet.BytesRecieved);
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
    }
}
