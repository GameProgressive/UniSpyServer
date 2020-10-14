using NatNegotiation.Entity.Enumerator;
using NatNegotiation.Entity.Structure.Packet;
using System;

namespace NatNegotiation.Entity.Structure
{
    public class NatNegUserInfo
    {
        public byte Version { get; protected set; }
        public uint Cookie { get; protected set; }
        public byte ClientIndex { get; protected set; }
        public NatPortType PortType { get; protected set; }

        public bool IsGotErtAckPacket { get; protected set; }
        public NatNegUserInfo SetIsGotErtACKPacketFlag(bool flag = true)
        {
            IsGotErtAckPacket = flag;
            return this;
        }
        public bool IsGotAddressCheckPacket { get; protected set; }
        public NatNegUserInfo SetIsGotAddressCheckPacketFlag(bool flag = true)
        {
            IsGotAddressCheckPacket = flag;
            return this;
        }
        public bool IsGotInitPacket { get; protected set; }
        public NatNegUserInfo SetIsGotInitPacketFlag(bool flag = true)
        {
            IsGotInitPacket = flag;
            return this;
        }
        public bool IsSendConnectPacket { get; protected set; }
        public bool IsGotConnectAckPacket { get; protected set; }
        public NatNegUserInfo SetIsGotConnectAckPacketFlag(bool flag = true)
        {
            IsGotConnectAckPacket = flag;
            return this;
        }
        public bool IsGotReportPacket { get; protected set; }
        public NatNegUserInfo SetIsGotReportPacketFlag(bool flag = true)
        {
            IsGotReportPacket = flag;
            return this;
        }
        public GameInfo Game { get; protected set; }
        public string GameName { get; protected set; }
        public NatNegUserInfo SetGameName(string gameName)
        {
            GameName = gameName;
            return this;
        }

        public DateTime InitPacketRecieveTime { get; protected set; }
        public NatNegUserInfo UpdateInitPacketRecieveTime()
        {
            InitPacketRecieveTime = DateTime.Now;
            LastPacketRecieveTime = DateTime.Now;
            IsGotInitPacket = true;
            return this;
        }

        public DateTime ConnectPacketSendTime { get; protected set; }
        public NatNegUserInfo UpdateConnectPacketSendTime()
        {
            ConnectPacketSendTime = DateTime.Now;
            LastPacketRecieveTime = DateTime.Now;
            IsSendConnectPacket = true;
            return this;
        }

        public DateTime LastPacketRecieveTime { get; protected set; }
        public NatNegUserInfo UpdateLastPacketReceiveTime()
        {
            LastPacketRecieveTime = DateTime.Now;
            return this;
        }

        public int RetryNatNegotiationTime { get; protected set; }

        public NatNegUserInfo UpdateRetryNatNegotiationTime()
        {
            RetryNatNegotiationTime++;
            return this;
        }

        public NatNegUserInfo()
        {
            LastPacketRecieveTime = DateTime.Now;
            RetryNatNegotiationTime = 0;
        }

        public void Parse(InitPacket init)
        {
            Version = init.Version;
            Cookie = init.Cookie;
            PortType = init.PortType;
            ClientIndex = init.ClientIndex;
        }


    }
}
