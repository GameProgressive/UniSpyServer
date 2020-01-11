using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;
using System.Linq;

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
            _result = OthersListQuery.GetOtherBuddyList(opids, Convert.ToUInt16(_recv["namespaceid"]));
        }

        protected override void ConstructResponse(GPSPSession session)
        {
            _sendingBuffer = @"\otherslist\";
            foreach (Dictionary<string, object> player in _result)
            {
                _sendingBuffer += @"\o\" + player["profileid"];
                _sendingBuffer += @"\uniquenick\" + player["uniquenick"];
            }
            _sendingBuffer += @"oldone\final\";
        }
    }
}
