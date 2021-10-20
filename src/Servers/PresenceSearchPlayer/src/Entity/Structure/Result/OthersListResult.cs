using UniSpyServer.PresenceSearchPlayer.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Structure.Result
{
    public sealed class OthersListDatabaseModel
    {
        public uint ProfileID;
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
