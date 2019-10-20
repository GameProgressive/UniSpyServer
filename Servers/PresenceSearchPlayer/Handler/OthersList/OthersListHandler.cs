using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Handler.OthersList
{
    public class OthersListHandler
    {
        static List<Dictionary<string, object>> _queryResult;
        static Dictionary<string, string> _recv;
        static GPErrorCode _errorCode;
        static string _sendingBuffer;
        static string _errorMsg;

        /// <summary>
        /// search a buddy list which contain less information
        /// </summary>
        /// <param name="session"></param>
        /// <param name="recv"></param>
        public static void SearchOtherBuddyList(GPSPSession session, Dictionary<string, string> recv)
        {
            _recv = recv;
            _errorCode = GPErrorCode.NoError;
            _sendingBuffer = "";
            _errorMsg = "";
            _queryResult = null;

            IsContainAllKey();
            if (_errorCode != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, _errorCode, _errorMsg);
                return;
            }
            uint[] opids = recv["opids"].TrimStart('|').Split('|').Select(uint.Parse).ToArray();

            _queryResult = OthersListQuery.GetOtherBuddyList(opids, Convert.ToUInt16(_recv["namespaceid"]));

            CheckDatabaseResult();
            if (_errorCode != GPErrorCode.NoError)
            {
                session.SendAsync(@"\otherslist\\odone\final\");
                return;
            }
            SendResponse(session);

            //request: \otherslist\sesskey\<searcher's sesskey>\profileid\<searcher's pid>\numopids\<how many pid in his list>
            //\opids\|<opid1>|<opid2>|******\namespaceid\<>\gamename\<>\final\
        }

        private static void CheckDatabaseResult()
        {
            if (_queryResult == null)
            {
                _errorCode = GPErrorCode.DatabaseError;
                _errorMsg = "No match found";
            }
        }

        private static void SendResponse(GPSPSession session)
        {
            _sendingBuffer = @"\otherslist\";
            foreach(Dictionary<string,object> player in _queryResult)
            {
                _sendingBuffer += @"\o\" + player["profileid"];
                _sendingBuffer += @"\uniquenick\" + player["uniquenick"];
            }
            _sendingBuffer += @"oldone\final\";
            session.SendAsync(_sendingBuffer);
        }

        private static void IsContainAllKey()
        {
            if (!_recv.ContainsKey("opids") || !_recv.ContainsKey("namespaceid"))
            {
                _errorCode = GPErrorCode.Parse;
                _errorMsg = "Parsing error";
            }
        }
    }
}
