using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;
using System;

namespace NATNegotiation.Entity.Structure
{
    public class NNUserInfo
    {
        public byte Version { get; protected set; }
        public uint Cookie { get; protected set; }
        public byte ClientIndex { get; protected set; }
        public NatPortType PortType { get; protected set; }

        public bool IsGotErtAckPacket { get; protected set; }
        public NNUserInfo SetIsGotErtACKPacketFlag(bool flag = true)
        {
            IsGotErtAckPacket = flag;
            return this;
        }
        public bool IsGotAddressCheckPacket { get; protected set; }
        public NNUserInfo SetIsGotAddressCheckPacketFlag(bool flag = true)
        {
            IsGotAddressCheckPacket = flag;
            return this;
        }
        public bool IsGotInitPacket { get; protected set; }
        public NNUserInfo SetIsGotInitPacketFlag(bool flag = true)
        {
            IsGotInitPacket = flag;
            return this;
        }
        public bool IsSendConnectPacket { get; protected set; }
        public bool IsGotConnectAckPacket { get; protected set; }
        public NNUserInfo SetIsGotConnectAckPacketFlag(bool flag = true)
        {
            IsGotConnectAckPacket = flag;
            return this;
        }
        public bool IsGotReportPacket { get; protected set; }
        public NNUserInfo SetIsGotReportPacketFlag(bool flag = true)
        {
            IsGotReportPacket = flag;
            return this;
        }
        public GameInfo Game { get; protected set; }
        public string GameName { get; protected set; }
        public NNUserInfo SetGameName(string gameName)
        {
            GameName = gameName;
            return this;
        }

        public DateTime InitPacketRecieveTime { get; protected set; }
        public NNUserInfo UpdateInitPacketRecieveTime()
        {
            InitPacketRecieveTime = DateTime.Now;
            LastPacketRecieveTime = DateTime.Now;
            IsGotInitPacket = true;
            return this;
        }

        public DateTime ConnectPacketSendTime { get; protected set; }
        public NNUserInfo UpdateConnectPacketSendTime()
        {
            ConnectPacketSendTime = DateTime.Now;
            LastPacketRecieveTime = DateTime.Now;
            IsSendConnectPacket = true;
            return this;
        }

        public DateTime LastPacketRecieveTime { get; protected set; }
        public NNUserInfo UpdateLastPacketReceiveTime()
        {
            LastPacketRecieveTime = DateTime.Now;
            return this;
        }

        public int RetryNATNegotiationTime { get; protected set; }

        public NNUserInfo UpdateRetryNATNegotiationTime()
        {
            RetryNATNegotiationTime++;
            return this;
        }

        public NNUserInfo()
        {
            LastPacketRecieveTime = DateTime.Now;
            RetryNATNegotiationTime = 0;
        }

        public void Parse(InitRequest init)
        {
            Version = init.Version;
            Cookie = init.Cookie;
            PortType = init.PortType;
            ClientIndex = init.ClientIndex;
        }


    }
}
