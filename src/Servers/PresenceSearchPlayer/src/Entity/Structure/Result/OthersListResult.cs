using PresenceSearchPlayer.Abstraction.BaseClass;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Entity.Structure.Result
{
    internal sealed class OthersListDatabaseModel
    {
        public uint ProfileID;
        public string Uniquenick;
    }
    internal sealed class OthersListResult : ResultBase
    {
        public List<OthersListDatabaseModel> DatabaseResults;
        public OthersListResult()
        {
            DatabaseResults = new List<OthersListDatabaseModel>();
        }
    }
}
