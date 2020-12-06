using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Entity.Structure.Request.Buddy;
using System.Linq;
using PresenceConnectionManager.Abstraction.BaseClass;

namespace PresenceConnectionManager.Handler.CommandHandler
{
    public class AddBlockHandler : PCMCommandHandlerBase
    {

        protected new AddBlockRequest _request { get { return (AddBlockRequest)base._request; } }
        public AddBlockHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
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
