using PresenceSearchPlayer.Abstraction.BaseClass;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Entity.Structure.Result
{
    internal sealed class SearchDataBaseModel
    {
        public uint Profileid;
        public string Nick;
        public string Uniquenick;
        public string Email;
        public string Firstname;
        public string Lastname;
        public uint NamespaceID;
    }

    internal sealed class SearchResult : ResultBase
    {
        public List<SearchDataBaseModel> DataBaseResults;
        public SearchResult()
        {
            DataBaseResults = new List<SearchDataBaseModel>();
        }
    }
}
