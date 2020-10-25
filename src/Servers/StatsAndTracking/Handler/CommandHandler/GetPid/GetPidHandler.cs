using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using StatsAndTracking.Entity.Enumerator;
using StatsAndTracking.Entity.Structure.Request;
using System.Collections.Generic;
using System.Linq;

namespace StatsAndTracking.Handler.CommandHandler.GetPID
{
    public class GetPIDHandler : GStatsCommandHandlerBase
    {
        //request \getpid\\nick\%s\keyhash\%s\lid\%d
        //response \getpidr
        private uint _protileid;
        protected GetPIDRequest _request;
        public GetPIDHandler(ISession session, Dictionary<string, string> request) : base(session, request)
        {
            _request = new GetPIDRequest(request);
        }

        protected override void CheckRequest()
        {
            _errorCode = _request.Parse();
        }

        protected override void DataOperation()
        {
            using (var db = new retrospyContext())
            {
                var result = from p in db.Profiles
                             join s in db.Subprofiles on p.Profileid equals s.Profileid
                             where s.Cdkeyenc == _request.KeyHash && p.Nick == _request.Nick
                             select s.Profileid;
                if (result.Count() != 1)
                {
                    _errorCode = GStatsErrorCode.Database;
                    return;
                }
                _protileid = result.First();
            }
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = $@"\getpidr\{_protileid}\lid\{ _request.OperationID}";
        }
    }
}
