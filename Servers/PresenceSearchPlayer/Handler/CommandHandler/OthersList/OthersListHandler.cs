using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CommandHandler.OthersList
{
    public class OthersListHandler : GPSPHandlerBase
    {
        public OthersListHandler(GPSPSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        //request: \otherslist\sesskey\<searcher's sesskey>\profileid\<searcher's pid>\numopids\<how many pid in his list>
        //\opids\|<opid1>|<opid2>|******\namespaceid\<>\gamename\<>\final\

        protected override void CheckRequest(GPSPSession session, Dictionary<string, string> recv)
        {
            base.CheckRequest(session, recv);
            if (!recv.ContainsKey("opids") || !recv.ContainsKey("namespaceid"))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void DataBaseOperation(GPSPSession session, Dictionary<string, string> recv)
        {
            uint[] opids = recv["opids"].TrimStart('|').Split('|').Select(uint.Parse).ToArray();
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
