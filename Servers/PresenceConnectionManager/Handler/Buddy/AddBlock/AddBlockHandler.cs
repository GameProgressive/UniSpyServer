using System;
using System.Collections.Generic;
using PresenceConnectionManager.Enumerator;

namespace PresenceConnectionManager.Handler.Buddy.AddBlock
{
    public class AddBlockHandler:GPCMHandlerBase
    {
        public AddBlockHandler(Dictionary<string, string> recv) : base(recv)
        {
        }
        protected override void CheckRequest(GPCMSession session)
        {
            base.CheckRequest(session);
            if (!_recv.ContainsKey("profileid"))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void DataBaseOperation(GPCMSession session)
        {
            AddBlockQuery.UpdateBlockListInDatabase(session.PlayerInfo.Profileid, Convert.ToUInt16(_recv["profileid"]), session.PlayerInfo.NamespaceID);
        }
    }
}
