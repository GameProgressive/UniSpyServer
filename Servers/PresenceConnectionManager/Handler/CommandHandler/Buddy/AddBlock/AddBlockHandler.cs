using GameSpyLib.Database.DatabaseModel.MySql;
using LinqToDB;
using PresenceConnectionManager.Enumerator;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.Buddy.AddBlock
{
    public class AddBlockHandler : GPCMHandlerBase
    {
        private uint _blockProfileid;
        public AddBlockHandler(GPCMSession session,Dictionary<string, string> recv) : base(session,recv)
        {
        }
        protected override void CheckRequest(GPCMSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session,recv);
            if (!recv.ContainsKey("profileid"))
            {
                _errorCode = GPErrorCode.Parse;
            }
            else if (!uint.TryParse(recv["profileid"], out _blockProfileid))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void DataBaseOperation(GPCMSession session, Dictionary<string, string> recv)
        {
            using (var db = new RetrospyDB())
            {
                if (db.Blockeds.Where(b => b.Targetid == _blockProfileid && b.Namespaceid == session.UserInfo.NamespaceID && b.Profileid == session.UserInfo.Profileid).Count() == 0)
                {
                    db.Blockeds
                        .Value(b => b.Profileid, session.UserInfo.Profileid)
                        .Value(b => b.Targetid, _blockProfileid)
                        .Value(b => b.Namespaceid, session.UserInfo.NamespaceID)
                        .Insert();
                }
            }
        }
    }
}
