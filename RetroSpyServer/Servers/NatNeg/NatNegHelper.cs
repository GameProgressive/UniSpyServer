using GameSpyLib.Network;
using GameSpyLib.Logging;
using RetroSpyServer.Servers.NatNeg.Structures;
using RetroSpyServer.Servers.NatNeg.Enumerators;

namespace RetroSpyServer.Servers.NatNeg
{
    class NatNegHelper
    {
        // NOTE: Do not handle this until we investigate the NatNeg Protocol Version 4 (We are supporting Version 3, the same as the SDK)
        public static void PreInitResponse(NatNegServer natNegServer, UdpPacket packet, NatNegPacket nnpacket)
        {
            LogWriter.Log.Write("[NATNEG] No impliment function for PreInitResponse!", LogLevel.Debug);

            //    ClientInfo clientInfo = new ClientInfo();
            //    if (clientInfo.GotPreInit)
            //        return;
            //    clientInfo.ClientID = nnpacket.Packet.PreInit.ClientID;
            //    clientInfo.ClientIndex = nnpacket.Packet.PreInit.ClientIndex;
        }

        public static void InitPacketResponse(NatNegServer server, UdpPacket packet, NatNegPacket nnpacket)
        {
            //when every time we add a client in to ClientInfo list, we have to add 1 on InstanceCount
            ClientInfo clientInfo = new ClientInfo
                (
                nnpacket.Common.Version,
                nnpacket.Common.Cookie,
                nnpacket.Init.ClientIndex,
                true,
                server.InstanceCount++
                );
            server.ClientInfoList.Add(clientInfo);

            //then we set the nnpacket that we recieved in to a reply format
            nnpacket.Common.PacketType = NatPacketType.InitAck;

            //we send the reply packet to client
            server.ReplyAsync(packet, nnpacket.ToBytes());
        }

        public static void AddressCheckResponse(NatNegServer server, UdpPacket packet, NatNegPacket nnpacket)
        {
            LogWriter.Log.Write("[NATNEG] No impliment function for AddressCheckPacket!", LogLevel.Debug);
            //server.ReplyAsync(packet, packet.BytesRecieved);
        }

        public static void NatifyResponse(NatNegServer server, UdpPacket packet, NatNegPacket nnpacket)
        {
            //LogWriter.Log.Write("[NATNEG] No impliment function for NatifyResponse!", LogLevel.Debug);
            server.ReplyAsync(packet, nnpacket.ToBytes());
            //server.ReplyAsync(packet, packet.BytesRecieved);
        }

        public static void ReportResponse(NatNegServer server, UdpPacket packet, NatNegPacket nnpacket)
        {
            LogWriter.Log.Write("[NATNEG] No impliment function for ReportResponse!", LogLevel.Debug);
            //server.ReplyAsync(packet, packet.BytesRecieved);
        }

        public static void SaveNatNegPacket(NatNegServer server, UdpPacket upacket, NatNegPacket nnpacket)
        {
            LogWriter.Log.Write("[NATNEG] No impliment function for SaveNatNegPacket!", LogLevel.Debug);

            //server._nnpacket.version = upacket.BytesRecieved[6];
            //server._nnpacket.packettype = upacket.BytesRecieved[7];


        }

        public static void NNConnectResponse(NatNegServer natNegServer, UdpPacket packet, NatNegPacket nnpacket)
        {
            // Cache the client info on the init packet and then access them with the cookie and send GotConnectAck to true

            /*ClientInfo clientinfo = new ClientInfo();
            clientinfo.GotConnectAck = true;*/
        }

    }
}
