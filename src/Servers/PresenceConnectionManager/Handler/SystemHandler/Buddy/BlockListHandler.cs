using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using PresenceConnectionManager.Abstraction.BaseClass;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Abstraction.SystemHandler
{
    public class BlockListHandler : PCMCommandHandlerBase
    {
        protected List<uint> _profileIDList;
        public BlockListHandler(ISession session, IRequest request) : base(session, request)
        {
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            // \bdy\< num in list >\list\< profileid list - comma delimited >\final\
            _sendingBuffer = $@"\bdy\{_profileIDList.Count()}\list\";
            foreach (var pid in _profileIDList)
            {
                _sendingBuffer += $@"{pid}";
                if (pid != _profileIDList.Last())
                {
                    _sendingBuffer += ",";
                }
            }
            _sendingBuffer += @"\final\";
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            using (var db = new retrospyContext())
            {
                _profileIDList = db.Blocked
                    .Where(f => f.Profileid == _session.UserData.ProfileID
                    && f.Namespaceid == _session.UserData.NamespaceID)
                    .Select(f => f.Targetid).ToList();
            }
        }
    }
}
