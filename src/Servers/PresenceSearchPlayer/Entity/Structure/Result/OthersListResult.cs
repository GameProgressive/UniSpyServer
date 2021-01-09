using System;
using System.Collections.Generic;
using PresenceSearchPlayer.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Result
{
    public class OthersListDatabaseModel
    {
        public uint ProfileID;
        public string Uniquenick;
    }
    public class OthersListResult : PSPResultBase
    {
        public List<OthersListDatabaseModel> DatabaseResults;
        public OthersListResult()
        {
            DatabaseResults = new List<OthersListDatabaseModel>();
        }
    }
}
