using NatNegotiation.Entity.Structure;
using NatNegotiation.Entity.Structure.Packet;
using System;
using System.Linq;
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
        public EndPoint RemoteEndPoint;
        public byte[] GameName;

        public byte[] PublicIP => ((IPEndPoint)RemoteEndPoint).Address.GetAddressBytes();

        public byte[] PublicPort => BitConverter.GetBytes(((IPEndPoint)RemoteEndPoint).Port).Take(2).ToArray();

        public byte[] InternalIP = new byte[4];

        public byte[] InternalPort = new byte[2];

        public DateTime ConnectPacketTime;
        public DateTime LastPacketTime;
        public DateTime SentConnectPacketTime;

        public ClientInfo TargetClient;

        public ClientInfo(EndPoint end)
        {
            RemoteEndPoint = end;
            LastPacketTime = DateTime.Now;
        }

        public void Parse(InitPacket init)
        {
            Version = init.Version;
            Cookie = init.Cookie;
            PortType = init.PortType;
            ClientIndex = init.ClientIndex;
            IsGotInit = true;
        }

        public void Parse(ConnectPacket connect)
        {

        }
    }
}
