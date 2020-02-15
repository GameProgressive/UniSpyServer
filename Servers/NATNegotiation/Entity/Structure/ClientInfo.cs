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
        public byte[] Cookie = new byte[4];
        public byte ClientIndex;
        public bool Connected;
        public bool GotInit;
        public bool GotConnectAck;
        public GameInfo Game;
        public EndPoint EndPoint;

        //public byte[] RemoteIP = new byte[4];
        //public byte[] RemotePort = new byte[2];
        //public byte GotYourData;
        //public byte Finished;
    }
}
