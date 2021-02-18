using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Response;
using PresenceConnectionManager.Entity.Structure.Result;
using System.Linq;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    internal sealed class BlockListHandler : PCMCmdHandlerBase
    {
        private new BlockListResult _result
        {
            get => (BlockListResult)base._result;
            set => base._result = value;
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
                    .Where(f => f.Profileid == _session.UserInfo.BasicInfo.ProfileID
                    && f.Namespaceid == _session.UserInfo.BasicInfo.NamespaceID)
                    .Select(f => f.Targetid).ToList();
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new BlockListResponse(null, _result);
        }
    }
}
