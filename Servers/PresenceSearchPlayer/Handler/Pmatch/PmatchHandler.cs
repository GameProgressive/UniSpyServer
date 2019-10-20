using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.Pmatch
{
    /// <summary>
    /// Search the all players in specific game
    /// </summary>
    public class PmatchHandler
    {
        private static List<Dictionary<string, object>> _queryResult;
        private static Dictionary<string, string> _recv;
        private static GPErrorCode _errorCode;
        private static string _errorMsg;
        private static string _sendingBuffer;
        public static void PlayerMatch(GPSPSession session, Dictionary<string, string> recv)
        {
            _recv = recv;
            _queryResult = null;
            _errorCode = GPErrorCode.NoError;
            _errorMsg = "";
            _sendingBuffer = "";

            //pmath\\sesskey\\profileid\\productid\\
            IsContainAllKey();
            if (_errorCode != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, _errorCode, _errorMsg);
                return;
            }
            _queryResult = PmatchQuery.PlayerMatch(Convert.ToUInt16(_recv["productid"]));
            CheckDatabaseResult();
            if (_errorCode != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, _errorCode, _errorMsg);
                return;
            }
            SendRespose(session);
        }

        private static void SendRespose(GPSPSession session)
        {
            for (int i = 0; i < _queryResult.Count; i++)
            {
                _sendingBuffer += @"\psr\"+_queryResult[i]["profileid"];
                _sendingBuffer += @"\status\" + _queryResult[i]["statstring"];
                _sendingBuffer += @"\nick\" + _queryResult[i]["nick"];
                _sendingBuffer += @"\statuscode\" + _queryResult[i]["status"];

            }
            _sendingBuffer += @"\psrdone\final\";
            session.SendAsync(_sendingBuffer);

        }

        private static void IsContainAllKey()
        {
            if (!_recv.ContainsKey("sesskey") || !_recv.ContainsKey("profileid") || !_recv.ContainsKey("productid"))
            {
                _errorCode = GPErrorCode.Parse;
                _errorMsg = "Parsing error";
            }
        }
        private static void CheckDatabaseResult()
        {
            if (_queryResult.Count < 1)
            {
                _errorCode = GPErrorCode.DatabaseError;
                _errorMsg = "No match found";
            }
        }
    }
}
