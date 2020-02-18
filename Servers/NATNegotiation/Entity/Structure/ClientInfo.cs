using NatNegotiation.Entity.Structure;
using NatNegotiation.Entity.Structure.Packet;
using NATNegotiation.Handler.SystemHandler;
using System;
using System.Net;

namespace NATNegotiation.Entity.Structure
{
    public class ClientInfo
    {
        public byte Version;
        public byte[] Cookie = new byte[4];
        public byte ClientIndex;
        public byte PortType;
        public bool IsConnected;
        public bool IsGotInit;
        public bool IsGotConnectAck;
        public bool IsGotErtAck;
        public bool IsGotReport;
        public GameInfo Game;
        public EndPoint EndPoint;
        public byte[] GameName;


        public byte[] PublicIP = new byte[4];
        public byte[] PublicPort = new byte[2];
        public byte[] InternalIP = new byte[4];
        public byte[] InternalPort = new byte[2];


        public DateTime ConnectTime;
        public DateTime LastPacketTime;
        public DateTime SentConnectPacketTime;

        public ClientInfo TargetClient;

        public void Parse(byte[] recv)
        {
            Version = recv[BasePacket.MagicData.Length];
            Array.Copy(NNFormat.IPToByte(this.EndPoint), PublicIP, 4);
            Array.Copy(NNFormat.PortToByte(this.EndPoint), PublicPort, 2);
            LastPacketTime = DateTime.Now;
        }
    }
}
