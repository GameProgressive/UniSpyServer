using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Enumerator;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.Buddy.AddBlock
{
    public class AddBlockHandler : CommandHandlerBase
    {
        private uint _blockProfileid;

        public AddBlockHandler() : base()
        {
        }

        protected override void CheckRequest(GPCMSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session, recv);

            if (!recv.ContainsKey("profileid"))
            {
                _errorCode = GPErrorCode.Parse;
            }

            else if (!uint.TryParse(recv["profileid"], out _blockProfileid))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void DataOperation(GPCMSession session, Dictionary<string, string> recv)
        {
            using (var db = new retrospyContext())
            {
                if (db.Blocked.Where(b => b.Targetid == _blockProfileid && b.Namespaceid == session.UserInfo.NamespaceID && b.Profileid == session.UserInfo.Profileid).Count() == 0)
                {
                    Blocked blocked = new Blocked
                    {
                        Profileid = session.UserInfo.Profileid,
                        Targetid = _blockProfileid,
                        Namespaceid = session.UserInfo.NamespaceID
                    };

                    db.Blocked.Update(blocked);
                }
            }
        }
    }
}
