using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace RetroSpyServer.Servers.NatNeg
{
    public class ClientNatNegInfo
    {
        public long connecttime;
        public long sentconnecttime;
        public GameInfo game;
        public int sd;
        //memcpy(&sockinfo, peer,sizeof(struct sockaddr_in));
        //memcpy(&punching, peer,sizeof(struct sockaddr_in));
        public long lastPacket;
        public byte version = 0;
        public int cookie = 0;
        public int instance;
        public byte cindex = 0;
        public bool connected = false;
        public bool gotInit = false;
        public bool gotConnectAck = false;
        public bool deleteMe = false;

        public ClientNatNegInfo(int sd, IPAddress ip, int instance)
        {

        }
    }

    
}
