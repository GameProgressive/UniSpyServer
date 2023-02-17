using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Result
{
    public sealed class OthersListDatabaseModel
    {
        public int ProfileId;
        public string Uniquenick;
    }
    public sealed class OthersListResult : ResultBase
    {
        public List<OthersListDatabaseModel> DatabaseResults;
        public OthersListResult()
        {
            DatabaseResults = new List<OthersListDatabaseModel>();
        }
    }
}
