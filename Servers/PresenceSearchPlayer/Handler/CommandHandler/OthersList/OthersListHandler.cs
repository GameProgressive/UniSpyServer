using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;
using System.Linq;
using GameSpyLib.Database.DatabaseModel.MySql;

namespace PresenceSearchPlayer.Handler.CommandHandler.OthersList
{
    public class OthersListHandler : GPSPHandlerBase
    {
        public OthersListHandler(Dictionary<string, string> recv) : base(recv)
        {
        }

        //request: \otherslist\sesskey\<searcher's sesskey>\profileid\<searcher's pid>\numopids\<how many pid in his list>
        //\opids\|<opid1>|<opid2>|******\namespaceid\<>\gamename\<>\final\

        protected override void CheckRequest(GPSPSession session)
        {
            base.CheckRequest(session);
            if (!_recv.ContainsKey("opids") || !_recv.ContainsKey("namespaceid"))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void DataBaseOperation(GPSPSession session)
        {
            uint[] opids = _recv["opids"].TrimStart('|').Split('|').Select(uint.Parse).ToArray();
            try
            {
                using (var db = new RetrospyDB())
                {
                    _sendingBuffer = @"\otherslist\";
                    foreach (var pid in opids)
                    {
                        var info = from n in db.Subprofiles
                                   where n.Profileid == pid && n.Namespaceid == _namespaceid
                                   select new { uniquenick = n.Uniquenick };
                        _sendingBuffer += @"\o\" + pid;
                        _sendingBuffer += @"\uniquenick\" + info.First().uniquenick;
                    }
                    _sendingBuffer += @"oldone\final\";
                }
            }
            catch
            {
                _errorCode = GPErrorCode.DatabaseError;
            }
        }
    }
}
