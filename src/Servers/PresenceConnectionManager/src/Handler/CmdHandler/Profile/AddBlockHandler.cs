using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request.Profile;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel.MySql;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler
{
    [HandlerContract("addblock")]
    public sealed class AddBlockHandler : Abstraction.BaseClass.CmdHandlerBase
    {
        private new AddBlockRequest _request => (AddBlockRequest)base._request;
        public AddBlockHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            using (var db = new UnispyContext())
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
