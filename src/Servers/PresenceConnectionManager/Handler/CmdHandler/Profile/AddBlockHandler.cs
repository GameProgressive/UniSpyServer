using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Entity.Structure.Request;
using System.Linq;
using PresenceConnectionManager.Abstraction.BaseClass;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    public class AddBlockHandler : PCMCmdHandlerBase
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
                && b.Namespaceid == _session.UserInfo.NamespaceID
                && b.Profileid == _session.UserInfo.ProfileID).Count() == 0)
                {
                    Blocked blocked = new Blocked
                    {
                        Profileid = _session.UserInfo.ProfileID,
                        Targetid = _request.ProfileID,
                        Namespaceid = _session.UserInfo.NamespaceID
                    };

                    db.Blocked.Update(blocked);
                }
            }
        }
    }
}
