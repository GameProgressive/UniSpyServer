using System.Collections.Generic;

namespace QueryReport.Entity.Structure.Server
{
    public class PeerGameServer
    {
        public string GameName;
        public int GroupID;
        public Dictionary<string, string> StandardKeyValues;
        public Dictionary<string, string> CustomKeyValues;

        public PeerGameServer()
        {
            StandardKeyValues = new Dictionary<string, string>();
            CustomKeyValues = new Dictionary<string, string>();
        }

        public void Parse(string gameName, int groupID)
        {
            GameName = gameName;
            GroupID = groupID;
        }
    }
}
