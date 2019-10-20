using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.Others
{
    /// <summary>
    /// Get buddy's information
    /// </summary>
    public class OthersHandler
    {
        static List<Dictionary<string, object>> _queryResult;
        static Dictionary<string, string> _recv;
        static GPErrorCode _errorCode;
        static string _sendingBuffer;
        static string _errorMsg;
        /// <summary>
        /// Get your friends profile detail
        /// </summary>
        /// <param name="session"></param>
        /// <param name="recv"></param>
        public static void SearchOtherBuddy(GPSPSession session, Dictionary<string, string> recv)
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
            _queryResult = OthersQuery.GetOtherBuddy(recv);
           
            CheckDatabaseReult();
            if (_errorCode != GPErrorCode.NoError)
            {
                session.SendAsync(@"\others\\odone\final\");
                return;
            }

            SendResponse(session);
        }

        private static void SendResponse(GPSPSession session)
        {            
            _sendingBuffer = @"\others\";
           foreach(Dictionary<string,object> player in _queryResult)
            {
                _sendingBuffer += @"\o\" + player["profileid"];
                _sendingBuffer += @"\nick\" + player["nick"];
                _sendingBuffer += @"\uniquenick\" + player["uniquenick"];
                _sendingBuffer += @"\first\" + player["firstname"];
                _sendingBuffer += @"\last\" + player["lastname"];
                _sendingBuffer += @"\email\" + player["email"];
            }
            _sendingBuffer += @"\odone\final\";
            session.SendAsync(_sendingBuffer);
        }

        private static void CheckDatabaseReult()
        {
            if (_queryResult == null)
            {
                _errorCode = GPErrorCode.DatabaseError;
                _errorMsg = "No match found";                
            }
        }

        private static void IsContainAllKey()
        {
            throw new NotImplementedException();
        }
    }
}
