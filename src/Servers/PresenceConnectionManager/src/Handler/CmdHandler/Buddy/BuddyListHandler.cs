using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Request;
using PresenceConnectionManager.Entity.Structure.Response;
using PresenceConnectionManager.Entity.Structure.Result;
using System.Linq;
using System.Threading.Tasks;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    public sealed class BuddyListHandler : CmdHandlerBase
    {
        private new BuddyListResult _result
        {
            get => (BuddyListResult)base._result;
            set => base._result = value;
        }
        public BuddyListHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new BuddyListResult();
        }

        protected override void DataOperation()
        {
            using (var db = new unispyContext())
            {
                var result = db.Friends
                    .Where(f => f.Profileid == _session.UserInfo.BasicInfo.ProfileID
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
                    ProfileID = profileID,
                    NameSpaceID = _session.UserInfo.BasicInfo.NamespaceID,
                    IsGetStatusInfo = true
                };
                new StatusInfoHandler(_session, request).Handle();
            });
        }
    }
}
