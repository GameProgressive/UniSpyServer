using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Abstraction.BaseClass;
using System.Linq;
using PresenceConnectionManager.Entity.Structure.Result;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    public class BuddyListHandler : PCMCmdHandlerBase
    {
        protected new BuddyListResult _result
        {
            get { return (BuddyListResult)base._result; }
            set { base._result = value; }
        }
        public BuddyListHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void RequestCheck()
        {
            _result = new BuddyListResult(_request);
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                _result.ProfileIdList = db.Friends
                    .Where(f => f.Profileid == _session.UserInfo.ProfileID
                    && f.Namespaceid == _session.UserInfo.NamespaceID)
                    .Select(f => f.Targetid).ToList();
            }
        }
    }
}
