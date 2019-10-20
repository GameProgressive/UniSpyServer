using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.SearchUnique
{
    /// <summary>
    /// Search with uniquenick and namespace
    /// </summary>
    public class SearchUniqueHandler
    {
        private static Dictionary<string, object> _queryResult;
        private static Dictionary<string, string> _recv;
        private static GPErrorCode _error;
        private static string _errorMsg;
        private static string _sendingBuffer;
        public static void SearchProfileWithUniquenick(GPSPSession session, Dictionary<string, string> recv)
        {
            _recv = recv;
            _sendingBuffer = "";
            _error = GPErrorCode.NoError;
            _errorMsg = "";
            _queryResult = null;

            IsContainAllKey();
            if (_error != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, _error, _errorMsg);
                return;
            }
            _queryResult = SeachUniqueQuery.GetProfileWithUniquenickAndNamespace(_recv["uniquenick"], Convert.ToUInt16(_recv["namespaceid"]));
            CheckDatabaseResult();
            if (_error != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, _error, _errorMsg);
                return;
            }
            SendResponse(session);
        }

        private static void CheckDatabaseResult()
        {
           if(_queryResult.Count==0)
            {
                _error = GPErrorCode.DatabaseError;
                _errorMsg = "No match found!";
            }
        }

        private static void IsContainAllKey()
        {
            if (!_recv.ContainsKey("uniquenick") || !_recv.ContainsKey("namespace"))
            {
                _error = GPErrorCode.Parse;
                _errorMsg = "Parsing error!";
            }
        }
        private static void SendResponse(GPSPSession session)
        {
            _sendingBuffer = @"\bsr\" + Convert.ToUInt16(_queryResult["profileid"]);
            _sendingBuffer += @"\nick\" + _queryResult["nick"];
            _sendingBuffer += @"\uniquenick\" + _queryResult["uniquenick"];
            _sendingBuffer += @"\namespaceid\" + Convert.ToUInt16(_queryResult["namespaceid"]);
            _sendingBuffer += @"\firstname\" + _queryResult["firstname"];
            _sendingBuffer += @"\lastname\" + _queryResult["lastname"];
            _sendingBuffer += @"\email\" + _queryResult["email"];
            _sendingBuffer += @"\bsrdone\\more\0"+ @"\final\";
            session.SendAsync(_sendingBuffer);
        }
    }
}

