﻿using GameSpyLib.Network;
using GameSpyLib.Network.UDP;
using NATNegotiation.Structure.Packet;

namespace NATNegotiation.Handler
{
    class NatifyHandler
    {
        public static void NatifyResponse(NatNegServer server, UDPPacket packet)
        {
            InitPacket initPacket = new InitPacket(packet.BytesRecieved);
            byte[] sendingBuffer = initPacket.CreateReplyPacket();
            server.Send(packet, sendingBuffer);
        }
    }
}