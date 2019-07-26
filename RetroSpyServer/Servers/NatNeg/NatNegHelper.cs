using GameSpyLib.Network;
using RetroSpyServer.Servers.NatNeg.Structures;

namespace RetroSpyServer.Servers.NatNeg
{
    class NatNegHelper
    {

        //public static void PreInitResponse(NatNegServer natNegServer, UdpPacket packet, NatNegPacket nnpacket)
        //{            
        //    ClientInfo clientInfo = new ClientInfo();
        //    if (clientInfo.GotPreInit)
        //        return;
        //    clientInfo.ClientID = nnpacket.Packet.PreInit.ClientID;
        //    clientInfo.ClientIndex = nnpacket.Packet.PreInit.ClientIndex;
        //}


        public static void InitPacketResponse(NatNegServer server, UdpPacket packet, NatNegPacket nnpacket)
        {
            //when every time we add a client in to ClientInfo list, we have to add 1 on InstanceCount
            ClientInfo clientInfo = new ClientInfo
                (
                nnpacket.Version,
                nnpacket.Cookie,
                nnpacket.Packet.Init.ClientIndex,
                true,
                server.InstanceCount++
                );
            server.ClientInfoList.Add(clientInfo);

            //then we set the nnpacket that we recieved in to a reply format
            nnpacket.PacketType = NNRequest.NN_INITACK;
            byte[] replyPacket = nnpacket.NatNegInitFormat(NatNegInfo.INITPACKET_SIZE);


            //we send the reply packet to client
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

            //server._nnpacket.version = upacket.BytesRecieved[6];
            //server._nnpacket.packettype = upacket.BytesRecieved[7];


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

        public static void NNConnectResponse(NatNegServer natNegServer, UdpPacket packet)
        {
            ClientInfo clientinfo = new ClientInfo();
            clientinfo.GotConnectAck = true;
        }

        /// <summary>
        /// Get repsonse packet size from natneg recieved packet type
        /// </summary>
        /// <param name="type">recieved packet type</param>
        /// <returns></returns>
        public static int GetResponsePacketSize(byte type)
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
