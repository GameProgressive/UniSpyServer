using GameSpyLib.Network;
using GameSpyLib.Logging;
using RetroSpyServer.Servers.NatNeg.Structures;
using RetroSpyServer.Servers.NatNeg.Enumerators;
using System;

namespace RetroSpyServer.Servers.NatNeg
{
    public class NatNegHandler
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

        public static void InitResponse(NatNegServer server, UdpPacket packet, NatNegPacket nnpacket)
        {
            //when every time we add a client in to ClientInfo list, we have to add 1 on InstanceCount
            ClientInfo clientInfo = new ClientInfo(nnpacket.Common.Version, nnpacket.Common.Cookie, nnpacket.Init.ClientIndex, true, server.InstanceCount++);
            server.ClientInfoList.Add(clientInfo);

            //then we set the nnpacket that we recieved in to a reply format
            byte[] TempBytes = new byte[nnpacket.GetReplyPacketSize()];
            NatNegInfo.MagicData.CopyTo(TempBytes, 0);
            TempBytes[nnpacket.magicDataLen] = nnpacket.Common.Version;
            TempBytes[nnpacket.magicDataLen + 1] = (byte)NatPacketType.InitAck;
            BitConverter.GetBytes(nnpacket.Common.Cookie).CopyTo(TempBytes, nnpacket.magicDataLen + 2);

            TempBytes[CommonInfo.Size] = nnpacket.Init.PortType;
            TempBytes[CommonInfo.Size + 1] = nnpacket.Init.ClientIndex;
            TempBytes[CommonInfo.Size + 2] = nnpacket.Init.UseGamePort;
            BitConverter.GetBytes(nnpacket.Init.LocalIp).CopyTo(TempBytes, CommonInfo.Size + 3);
            BitConverter.GetBytes(nnpacket.Init.LocalPort).CopyTo(TempBytes, CommonInfo.Size + 7);
            //we send the reply packet to client
            server.SendAsync(packet, TempBytes);
        }

        public static void AddressCheckResponse(NatNegServer server, UdpPacket packet, NatNegPacket nnpacket)
        {

            byte[] TempBytes = new byte[nnpacket.GetReplyPacketSize()];
            NatNegInfo.MagicData.CopyTo(TempBytes, 0);

            TempBytes[nnpacket.magicDataLen] = nnpacket.Common.Version;
            TempBytes[nnpacket.magicDataLen + 1] = (byte)nnpacket.Common.PacketType;
            BitConverter.GetBytes(nnpacket.Common.Cookie).CopyTo(TempBytes, nnpacket.magicDataLen + 2);

            TempBytes[CommonInfo.Size] = (byte)NatPacketType.AddressReply;
            BitConverter.GetBytes(nnpacket.Init.LocalIp).CopyTo(TempBytes, CommonInfo.Size + 3);
            BitConverter.GetBytes(nnpacket.Init.LocalPort).CopyTo(TempBytes, CommonInfo.Size + 7);
            server.SendAsync(packet, TempBytes);
        }

        public static void NatifyResponse(NatNegServer server, UdpPacket packet, NatNegPacket nnpacket)
        {
            byte[] TempBytes = new byte[nnpacket.GetReplyPacketSize()];
            NatNegInfo.MagicData.CopyTo(TempBytes, 0);

            TempBytes[nnpacket.magicDataLen] = nnpacket.Common.Version;
            TempBytes[nnpacket.magicDataLen + 1] = (byte)nnpacket.Common.PacketType;
            BitConverter.GetBytes(nnpacket.Common.Cookie).CopyTo(TempBytes, nnpacket.magicDataLen + 2);

            TempBytes[CommonInfo.Size] = nnpacket.Init.PortType;
            TempBytes[CommonInfo.Size + 1] = nnpacket.Init.ClientIndex;
            TempBytes[CommonInfo.Size + 2] = nnpacket.Init.UseGamePort;
            BitConverter.GetBytes(nnpacket.Init.LocalIp).CopyTo(TempBytes, CommonInfo.Size + 3);
            BitConverter.GetBytes(nnpacket.Init.LocalPort).CopyTo(TempBytes, CommonInfo.Size + 7);
            server.SendAsync(packet, TempBytes);
        }

        public static void ReportResponse(NatNegServer server, UdpPacket packet, NatNegPacket nnpacket)
        {
            byte[] TempBytes = new byte[nnpacket.GetReplyPacketSize()];
            NatNegInfo.MagicData.CopyTo(TempBytes, 0);
            TempBytes[nnpacket.magicDataLen] = nnpacket.Common.Version;
            TempBytes[nnpacket.magicDataLen + 1] = (byte)nnpacket.Common.PacketType;
            BitConverter.GetBytes(nnpacket.Common.Cookie).CopyTo(TempBytes, nnpacket.magicDataLen + 2);

            TempBytes[CommonInfo.Size] = nnpacket.Report.PortType;
            TempBytes[CommonInfo.Size + 1] = nnpacket.Report.ClientIndex;
            TempBytes[CommonInfo.Size + 2] = nnpacket.Report.NegResult;
            BitConverter.GetBytes((int)nnpacket.Report.NatType).CopyTo(TempBytes, CommonInfo.Size + 3);
            BitConverter.GetBytes((int)nnpacket.Report.NatMappingScheme).CopyTo(TempBytes, CommonInfo.Size + 7);
            nnpacket.Report.GameName.CopyTo(TempBytes, CommonInfo.Size + 11);
            server.SendAsync(packet, TempBytes);
        }


        public static void ConnectResponse(NatNegServer server, UdpPacket packet, NatNegPacket nnpacket)
        {
            byte[] TempBytes = new byte[nnpacket.GetReplyPacketSize()];
            NatNegInfo.MagicData.CopyTo(TempBytes, 0);
            TempBytes[nnpacket.magicDataLen] = nnpacket.Common.Version;
            TempBytes[nnpacket.magicDataLen + 1] = (byte)nnpacket.Common.PacketType;
            BitConverter.GetBytes(nnpacket.Common.Cookie).CopyTo(TempBytes, nnpacket.magicDataLen + 2);

            // Cache the client info on the init packet and then access them with the cookie and send GotConnectAck to true
            BitConverter.GetBytes(nnpacket.Connect.RemoteIP).CopyTo(TempBytes, CommonInfo.Size);
            BitConverter.GetBytes(nnpacket.Connect.RemotePort).CopyTo(TempBytes, CommonInfo.Size + 4);

            TempBytes[CommonInfo.Size] = nnpacket.Connect.GotYourData;
            TempBytes[CommonInfo.Size + 1] = nnpacket.Connect.Finished;
            server.SendAsync(packet, TempBytes);
        }

    }
}
