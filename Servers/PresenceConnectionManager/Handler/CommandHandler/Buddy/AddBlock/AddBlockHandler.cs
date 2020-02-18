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
            else if (!uint.TryParse(_recv["profileid"], out _blockProfileid))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void DataBaseOperation(GPCMSession session)
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
