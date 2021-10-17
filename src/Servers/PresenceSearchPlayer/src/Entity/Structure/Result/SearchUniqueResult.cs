using PresenceSearchPlayer.Abstraction.BaseClass;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Entity.Structure.Result
{
    internal sealed class SearchUniqueDatabaseModel
    {
        public uint Profileid;
        public string Nick;
        public string Uniquenick;
        public string Email;
        public string Firstname;
        public string Lastname;
        public uint NamespaceID;
    }

    internal sealed class SearchUniqueResult : ResultBase
    {
        public List<SearchUniqueDatabaseModel> DatabaseResults;
        public SearchUniqueResult()
        {
            DatabaseResults = new List<SearchUniqueDatabaseModel>();
        }
    }
}
