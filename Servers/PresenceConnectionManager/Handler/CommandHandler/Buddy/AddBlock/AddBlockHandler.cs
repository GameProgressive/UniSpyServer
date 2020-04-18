using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Enumerator;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.Buddy.AddBlock
{
    public class AddBlockHandler : PCMCommandHandlerBase
    {
        protected uint _blockProfileid;

        public AddBlockHandler(IClient client, Dictionary<string, string> recv) : base(client, recv)
        {
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();

            if (!_recv.ContainsKey("profileid"))
            {
                _errorCode = GPErrorCode.Parse;
            }

            else if (!uint.TryParse(_recv["profileid"], out _blockProfileid))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                if (db.Blocked.Where(b => b.Targetid == _blockProfileid
                && b.Namespaceid == _session.UserInfo.NamespaceID
                && b.Profileid == _session.UserInfo.Profileid).Count() == 0)
                {
                    Blocked blocked = new Blocked
                    {
                        Profileid = _session.UserInfo.Profileid,
                        Targetid = _blockProfileid,
                        Namespaceid = _session.UserInfo.NamespaceID
                    };

                    db.Blocked.Update(blocked);
                }
            }
        }
    }
}
