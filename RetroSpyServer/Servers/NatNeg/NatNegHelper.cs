using System;
using System.Text;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using RetroSpyServer.Servers.NatNeg.Structures;

namespace RetroSpyServer.Servers.NatNeg
{
    class NatNegHelper
    {
        internal static void PreInitPacketResponse(NatNegServer server, UdpPacket packet)
        {
            LogWriter.Log.Write("heartbeat response packets " + packet.BytesRecieved.ToString(), LogLevel.Debug);
            //packet.SetBufferContents(NNMagicData.MagicData);
            //server.ReplyAsync(packet);
        }

        internal static void InitPacketResponse(NatNegServer server, UdpPacket packet)
        {
            LogWriter.Log.Write("heartbeat response packets " + packet.BytesRecieved.ToString(), LogLevel.Debug);
        }

        internal static void AddressCheckResponse(NatNegServer server, UdpPacket packet)
        {
            LogWriter.Log.Write("heartbeat response packets " + packet.BytesRecieved.ToString(), LogLevel.Debug);
        }

        internal static void NatifyResponse(NatNegServer server, UdpPacket packet)
        {
            LogWriter.Log.Write("heartbeat response packets " + packet.BytesRecieved.ToString(), LogLevel.Debug);
        }

        internal static void ReportResponse(NatNegServer server, UdpPacket packet)
        {
            LogWriter.Log.Write("heartbeat response packets " + packet.BytesRecieved.ToString(), LogLevel.Debug);
        }
    }
}
