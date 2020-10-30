using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Entity.Structure.Request.Buddy;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Abstraction.BaseClass.Profile
{
    public class AddBlockHandler : PCMCommandHandlerBase
    {

        protected AddBlockRequest _request;
        public AddBlockHandler(ISession session, Dictionary<string, string> recv) : base(session, recv)
        {
            _request = new AddBlockRequest(recv);
        }

        protected override void CheckRequest()
        {
            _errorCode = _request.Parse();
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                if (db.Blocked.Where(b => b.Targetid == _request.ProfileID
                && b.Namespaceid == _session.UserData.NamespaceID
                && b.Profileid == _session.UserData.ProfileID).Count() == 0)
                {
                    Blocked blocked = new Blocked
                    {
                        Profileid = _session.UserData.ProfileID,
                        Targetid = _request.ProfileID,
                        Namespaceid = _session.UserData.NamespaceID
                    };

                    db.Blocked.Update(blocked);
                }
            }
        }
    }
}
