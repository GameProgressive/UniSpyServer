using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RetroSpyServer.Servers.NatNeg
{
    public class GameData
    {
        public int gameid;
        public int queryport;
        public byte[] gamename = new byte[32];
        public byte[] description= new byte[64];
        public byte[] secretkey=new byte[7];
        public byte disabled_services; //0= ok, 1 = temp, 2 = perm
        public int compatibility_flags;
        public  static List<string> popularValues = new List<string>();
        public static Dictionary<string, byte> pushKeys = new Dictionary<string, byte>();
    }
}
