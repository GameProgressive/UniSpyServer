using System;
using System.Collections.Generic;
using System.Net;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;
using UniSpyLib.Logging;

namespace NATNegotiation.Entity.Structure
{
    public class NatUserInfo
    {
        public string RemoteEndPoint;
        public byte Version;
        public uint Cookie;
        public byte ClientIndex;
        public NatPortType PortType;
        public List<NatPortType> PortTypes;
        public bool IsGotErtAckPacket;
        public bool IsGotAddressCheckPacket;
        public bool IsGotInitPacket;
        public bool IsSendConnectPacket;
        public bool IsGotConnectAckPacket;
        public bool IsGotReportPacket;
        public GameInfo Game;
        public string GameName;
        public DateTime InitPacketRecieveTime;
        public DateTime ConnectPacketSendTime;
        public DateTime LastPacketRecieveTime;
        public int RetryNATNegotiationTime;

        public NatUserInfo()
        {
            PortTypes = new List<NatPortType>();
        }

        public void UpdateRemoteEndPoint(EndPoint endPoint)
        {
            RemoteEndPoint = ((IPEndPoint)endPoint).ToString();
        }

        public void UpdateInitRequestInfo(InitRequest init)
        {
            Version = init.Version;
            Cookie = init.Cookie;
            PortType = init.PortType;
            UpdatePortType(init.PortType);
            ClientIndex = init.ClientIndex;
        }

        public void UpdatePortType(NatPortType portType)
        {
            if (!PortTypes.Contains(portType))
            {
                PortTypes.Add(portType);
            }
            else
            {
                LogWriter.ToLog($"Ignore same port type{portType}");
            }
            LastPacketRecieveTime = DateTime.Now;
        }
    }
}
