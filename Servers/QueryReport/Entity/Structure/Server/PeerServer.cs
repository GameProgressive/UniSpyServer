using System;
using System.Collections.Generic;

namespace QueryReport.Entity.Structure.Server
{
    public class PeerServer
    {
        public string GameName;
        public int GroupID;
        public Dictionary<string, string> StandardKeyValues;
        public Dictionary<string, string> CustomKeyValues;

        public PeerServer()
        {
        }
    }
}
