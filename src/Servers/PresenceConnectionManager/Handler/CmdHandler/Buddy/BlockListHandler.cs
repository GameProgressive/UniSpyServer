using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Result;
using System.Linq;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    internal class BlockListHandler : PCMCmdHandlerBase
    {
        protected new BlockListResult _result
        {
            get { return (BlockListResult)base._result; }
            set { base._result = value; }
        }
        public BlockListHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new BlockListResult();
        }
        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                _result.ProfileIdList = db.Blocked
                    .Where(f => f.Profileid == _session.UserInfo.ProfileID
                    && f.Namespaceid == _session.UserInfo.NamespaceID)
                    .Select(f => f.Targetid).ToList();
            }
        }
    }
}
