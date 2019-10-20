using GameSpyLib.Common;
using GameSpyLib.Logging;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;

/////////////////////////Finished/////////////////////////////////
namespace PresenceSearchPlayer.Handler.Nick
{

    /// <summary>
    /// Uses a email and namespaceid to find all nick in this account
    /// </summary>
    public class NickHandler
    {
        static List<Dictionary<string, object>>_queryResult;
        static Dictionary<string, string> _recv;
        static GPErrorCode _errorCode;
        static string _sendingBuffer;
        static string _errorMsg;

        /// <summary>
        /// Get nickname through email and password
        /// </summary>
        /// <param name="session"></param>
        /// <param name="_recv"></param>
        public static void SearchNicks(GPSPSession session, Dictionary<string, string> recv)
        {
            _recv = recv;
            _errorCode = GPErrorCode.NoError;
            _sendingBuffer = "";
            _errorMsg = "";
            _queryResult = null;

            //Format the password for our database storage
            //if not recieved correct request we terminate
            IsContianAllKeys();
            if (_errorCode != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, _errorCode, _errorMsg);
                return;
            }
            //get nicknames from GPSPDBQuery class
            _queryResult = NickQuery.RetriveNicknames(_recv["email"],_recv["password"],Convert.ToUInt16(_recv["namespaceid"]));
            CheckDatabaseResult();
            if (_errorCode != GPErrorCode.NoError)
            {
                session.SendAsync(@"\nr\\ndone\final\");
                return;
            }

            SendResponse(session);           
        }

        private static void SendResponse(GPSPSession session)
        {
            _sendingBuffer= @"\nr\";
            foreach (Dictionary<string, object> player in _queryResult)
            {
                _sendingBuffer += @"\nick\";
                _sendingBuffer += player["nick"];
                _sendingBuffer += @"\uniquenick\";
                _sendingBuffer += player["uniquenick"];
            }
            _sendingBuffer += @"\ndone\final\";
            session.SendAsync(_sendingBuffer);
        }

        private static void CheckDatabaseResult()
        {
            if (_queryResult == null)
            {
                _errorCode = GPErrorCode.DatabaseError;
                _errorMsg = "No match found";
            }
        }

        public static void IsContianAllKeys()
        {
            if (!_recv.ContainsKey("email"))
            {
                _errorCode = GPErrorCode.Parse;
                _errorMsg = "Parsing error";
            }
            // First, we try to receive an encoded password
            if (!_recv.ContainsKey("passenc"))
            {
                // If the encoded password is not sended, we try receiving the password in plain text
                if (!_recv.ContainsKey("pass"))
                {
                    // No password is specified, we cannot continue                   
                    _errorCode = GPErrorCode.Parse;
                    _errorMsg = "Parsing error";
                }
                if (!_recv.ContainsKey("namespaceid"))
                {
                    _recv.Add("namespaceid", "0");
                }
            }
        }
    }
}
