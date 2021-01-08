using System.Collections.Generic;
using PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Result
{
    public class OthersDatabaseModel
    {
        public uint Profileid;
        public string Nick;
        public string Uniquenick;
        public string Lastname;
        public string Firstname;
        public uint Userid;
        public string Email;
    }

    public class OthersResult : PSPResultBase
    {
        public List<OthersDatabaseModel> DatabaseResults { get; set; }
        public OthersResult()
        {
        }

        public OthersResult(UniSpyRequestBase request) : base(request)
        {
            DatabaseResults = new List<OthersDatabaseModel>();
        }
    }
}
