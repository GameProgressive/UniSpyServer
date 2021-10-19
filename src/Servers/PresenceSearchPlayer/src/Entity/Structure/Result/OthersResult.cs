using PresenceSearchPlayer.Abstraction.BaseClass;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Entity.Structure.Result
{
    public sealed class OthersDatabaseModel
    {
        public uint Profileid;
        public string Nick;
        public string Uniquenick;
        public string Lastname;
        public string Firstname;
        public uint Userid;
        public string Email;
    }

    public sealed class OthersResult : ResultBase
    {
        public List<OthersDatabaseModel> DatabaseResults { get; set; }
        public OthersResult()
        {
            DatabaseResults = new List<OthersDatabaseModel>();
        }
    }
}
