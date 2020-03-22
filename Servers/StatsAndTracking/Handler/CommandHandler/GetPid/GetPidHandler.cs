using System;
using System.Collections.Generic;
using GameSpyLib.Database.DatabaseModel.MySql;
using StatsAndTracking.Entity.Enumerator;
using System.Linq;

namespace StatsAndTracking.Handler.CommandHandler.GetPid
{
    public class GetPidHandler:CommandHandlerBase
    {
        //\getpid\\nick\%s\keyhash\%s\lid\%d
        //\getpidr
        private uint _protileid;
        public GetPidHandler(GStatsSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }
        
        protected override void CheckRequest(GStatsSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session, recv);
            if (!recv.ContainsKey("nick")||!recv.ContainsKey("keyhash"))
            {
                _errorCode = GstatsErrorCode.Parse;
                return;
            }
        }

        protected override void DataOperation(GStatsSession session, Dictionary<string, string> recv)
        {
            base.DataOperation(session, recv);
            using (var db = new retrospyContext())
            {
                var result = from p in db.Profiles
                             join s in db.Subprofiles on p.Profileid equals s.Profileid
                             where s.Cdkeyenc == recv["cdkeyhash"] && p.Nick == recv["nick"]
                             select s.Profileid;
                if (result.Count() != 1)
                {
                    _errorCode = GstatsErrorCode.Database;
                    return;
                }
                _protileid = result.First();
            }
        }

        protected override void ConstructResponse(GStatsSession session, Dictionary<string, string> recv)
        {
            _sendingBuffer = $@"\getpidr\{_protileid}\lid\{_localId}";
        }
    }
}
