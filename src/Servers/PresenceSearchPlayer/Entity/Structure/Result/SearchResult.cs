using System;
using System.Collections.Generic;
using PresenceSearchPlayer.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Result
{
    public class SearchDataBaseModel
    {
        public uint Profileid;
        public string Nick;
        public string Uniquenick;
        public string Email;
        public string Firstname;
        public string Lastname;
        public uint NamespaceID;
    }

    public class SearchResult : PSPResultBase
    {
        public List<SearchDataBaseModel> DataBaseResults;
        public SearchResult()
        {
            DataBaseResults = new List<SearchDataBaseModel>();
        }
    }
}
