using UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Result
{
    public sealed class SearchDataBaseModel
    {
        public uint Profileid;
        public string Nick;
        public string Uniquenick;
        public string Email;
        public string Firstname;
        public string Lastname;
        public uint NamespaceID;
    }

    public sealed class SearchResult : ResultBase
    {
        public List<SearchDataBaseModel> DataBaseResults;
        public SearchResult()
        {
            DataBaseResults = new List<SearchDataBaseModel>();
        }
    }
}
