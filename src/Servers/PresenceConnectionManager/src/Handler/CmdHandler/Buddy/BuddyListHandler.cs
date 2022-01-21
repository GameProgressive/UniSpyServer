using System.Linq;
using System.Threading.Tasks;
using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request.Buddy;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler
{
    public sealed class BuddyListHandler : CmdHandlerBase
    {
        private new BuddyListResult _result { get => (BuddyListResult)base._result; set => base._result = value; }
        public BuddyListHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new BuddyListResult();
        }
        protected override void RequestCheck() { }
        protected override void DataOperation()
        {
            using (var db = new UniSpyContext())
            {
                var result = db.Friends
                    .Where(f => f.ProfileId == _session.UserInfo.BasicInfo.ProfileId
                    && f.Namespaceid == _session.UserInfo.BasicInfo.NamespaceID)
                    .Select(f => f.Targetid).ToList();
            }
        }
        protected override void ResponseConstruct()
        {
            _response = new BuddyListResponse(_request, _result);
        }
        protected override void Response()
        {
            base.Response();
            if (!_session.UserInfo.SDKRevision.IsSupportGPINewStatusNotification)
            {
                return;
            }

            Parallel.ForEach(_result.ProfileIDList, (profileID) =>
            {
                var request = new StatusInfoRequest
                {
                    ProfileId = profileID,
                    NamespaceID = _session.UserInfo.BasicInfo.NamespaceID,
                    IsGetStatusInfo = true
                };
                new StatusInfoHandler(_session, request).Handle();
            });
        }
    }
}
