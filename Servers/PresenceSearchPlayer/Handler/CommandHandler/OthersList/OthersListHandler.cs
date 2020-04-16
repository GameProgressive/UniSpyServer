using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.CommandHandler.OthersList
{
    public class OthersListHandler : PSPCommandHandlerBase
    {
        public OthersListHandler(IClient client, Dictionary<string, string> recv) : base(client, recv)
        {
        }

        //request: \otherslist\sesskey\<searcher's sesskey>\profileid\<searcher's pid>\numopids\<how many pid in his list>
        //\opids\|<opid1>|<opid2>|******\namespaceid\<>\gamename\<>\final\

        protected override void CheckRequest()
        {
            base.CheckRequest();

            if (!_recv.ContainsKey("opids") || !_recv.ContainsKey("namespaceid"))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void DataOperation()
        {
            uint[] opids = _recv["opids"].TrimStart('|').Split('|').Select(uint.Parse).ToArray();

            try
            {
                using (var db = new retrospyContext())
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
