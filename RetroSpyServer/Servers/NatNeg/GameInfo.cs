using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.NatNeg
{
    public class GameInfo
    {
        public int id;
        public byte name;
        public byte secretkey;
        public ushort queryport;
        public ushort backendflags;
        public uint servicesdisabled;
        public KeyData pushKeys;
        public byte numPushKeys; //sb protocol sends as a byte so max of 255
    }

    public class KeyData
    {
        public byte name;
        public byte type;
    }
}
