using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Request;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    [Command("addblock")]
    internal sealed class AddBlockHandler : PCMCmdHandlerBase
    {
        private new AddBlockRequest _request => (AddBlockRequest)base._request;
        public AddBlockHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            using (var db = new unispyContext())
            {
                if (db.Blocked.Where(b => b.Targetid == _request.ProfileID
                && b.Namespaceid == _session.UserInfo.BasicInfo.NamespaceID
                && b.Profileid == _session.UserInfo.BasicInfo.ProfileID).Count() == 0)
                {
                    Blocked blocked = new Blocked
                    {
                        Profileid = _session.UserInfo.BasicInfo.ProfileID,
                        Targetid = _request.ProfileID,
                        Namespaceid = _session.UserInfo.BasicInfo.NamespaceID
                    };

                    db.Blocked.Update(blocked);
                }
            }
        }
    }
}
