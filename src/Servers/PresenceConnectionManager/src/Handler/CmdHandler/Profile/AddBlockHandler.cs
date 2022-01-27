using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;

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
            using (var db = new UniSpyContext())
            {
                if (db.Blockeds.Where(b => b.Targetid == _request.ProfileId
                && b.Namespaceid == _session.UserInfo.BasicInfo.NamespaceId
                && b.ProfileId == _session.UserInfo.BasicInfo.ProfileId).Count() == 0)
                {
                    Blocked blocked = new Blocked
                    {
                        ProfileId = (int)_session.UserInfo.BasicInfo.ProfileId,
                        Targetid = _request.ProfileId,
                        Namespaceid = (int)_session.UserInfo.BasicInfo.NamespaceId
                    };

                    db.Blockeds.Update(blocked);
                }
            }
        }
    }
}
