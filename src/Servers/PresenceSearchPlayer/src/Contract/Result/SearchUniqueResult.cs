using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpy.Server.PresenceSearchPlayer.Contract.Result
{
    public sealed class SearchUniqueDatabaseModel
    {
        public int ProfileId;
        public string Nick;
        public string Uniquenick;
        public string Email;
        public string Firstname;
        public string Lastname;
        public int NamespaceID;
    }

    public sealed class SearchUniqueResult : ResultBase
    {
        public List<SearchUniqueDatabaseModel> DatabaseResults { get; set; }
        public SearchUniqueResult()
        {
        }
    }
}
