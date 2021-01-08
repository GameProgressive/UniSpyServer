using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Abstraction.BaseClass;
using System.Collections.Generic;
using System.Linq;
using PresenceConnectionManager.Entity.Structure.Result;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    public class BlockListHandler : PCMCmdHandlerBase
    {
        protected List<uint> _profileIDList;
        protected new BlockListResult _result
        {
            get { return (BlockListResult)base._result; }
            set { base._result = value; }
        }
        public BlockListHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
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
