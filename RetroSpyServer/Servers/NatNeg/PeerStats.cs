using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.NatNeg.Structures.Packets
{
    public class PeerStats
    {
        int pending_requests;
        int version;

        public long  bytes_in;
        public long  bytes_out;

        int packets_in;
        int packets_out;

       Address m_address;
       public  GameData from_game;

        bool disconnected;
    }
}
