using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Result;
using System.Linq;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    internal class BuddyListHandler : PCMCmdHandlerBase
    {
        protected new BuddyListResult _result
        {
            get { return (BuddyListResult)base._result; }
            set { base._result = value; }
        }
        public BuddyListHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new BuddyListResult();
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
