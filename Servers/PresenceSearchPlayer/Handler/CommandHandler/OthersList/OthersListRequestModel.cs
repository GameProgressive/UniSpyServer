using System;
using System.Collections.Generic;
using System.Linq;
using PresenceSearchPlayer.Entity.Structure.Model;
using PresenceSearchPlayer.Enumerator;

namespace PresenceSearchPlayer.Handler.CommandHandler.OthersList
{
    public class OthersListRequestModel : RequestModelBase
    {
        public List<uint> ProfileIDs { get; protected set; }

        public OthersListRequestModel(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GPErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!_recv.ContainsKey("opids") || !_recv.ContainsKey("namespaceid"))
            {
                return GPErrorCode.Parse;
            }

            ProfileIDs = _recv["opids"].TrimStart('|').Split('|').Select(uint.Parse).ToList();

            return GPErrorCode.NoError;

        }
    }
}
