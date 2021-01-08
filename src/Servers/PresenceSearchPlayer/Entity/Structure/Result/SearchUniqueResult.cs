using System;
using System.Collections.Generic;
using PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Result
{
    public class SearchUniqueDatabaseModel
    {
        public uint Profileid;
        public string Nick;
        public string Uniquenick;
        public string Email;
        public string Firstname;
        public string Lastname;
        public uint NamespaceID;
    }

    public class SearchUniqueResult : PSPResultBase
    {
        public List<SearchUniqueDatabaseModel> DatabaseResults;
        public SearchUniqueResult()
        {
        }

        public SearchUniqueResult(UniSpyRequestBase request) : base(request)
        {
            DatabaseResults = new List<SearchUniqueDatabaseModel>();
        }
    }
}
