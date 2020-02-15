using NatNegotiation.Entity.Structure;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace NATNegotiation.Entity.Structure
{
    public class ClientInfo
    {
        public byte Version;
        public DateTime ConnectTime;
        public DateTime LastPacketTime;
        public DateTime SentConnectPacketTime;
        public int Cookie;
        public byte ClientIndex;
        public bool Connected;
        public bool GotInit;
        public bool GotConnectAck;
        public GameInfo Game;
        public EndPoint EndPoint;

        //public uint RemoteIP;
        //public uint RemotePort;
        //public byte GotYourData;
        //public byte Finished;
    }
}
