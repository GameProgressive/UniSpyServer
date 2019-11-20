using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.Check
{
    public class CheckHandler
    {
        static Dictionary<string, object> _queryResult;
        static Dictionary<string, string> _recv;
        static GPErrorCode _errorCode;
        static string _sendingBuffer;
        static string _errorMsg;
        /// <summary>
        /// Validates a user's info, without logging into the account.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="recv"></param>
        public static void CheckProfileid(GPSPSession session, Dictionary<string, string> recv)
        {
            _recv = recv;
            _errorCode = GPErrorCode.NoError;
            _sendingBuffer = "";
            _errorMsg = "";
            // \check\\nick\<nick>\email\<email>\partnerid\0\passenc\<passenc>\gamename\gmtest\final\
            //\cur\pid\<pid>\final
            //check is request recieved correct and convert password into our MD5 type
            IsContainAllKey();
            if (_errorCode != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, _errorCode, _errorMsg);
                return;
            }

            _queryResult = CheckQuery.GetProfileidFromNickEmailPassword(_recv["email"], _recv["passenc"], _recv["nick"]);

            CheckDatabaseResult();

            if (_errorCode != GPErrorCode.NoError)
            {
                _sendingBuffer = @"\cur\" + GPErrorCode.CheckBadNick + @"\final\";
            }
            SendResponse(session);           
        }

        private static void SendResponse(GPSPSession session)
        {
            _sendingBuffer = @"\cur\" + GPErrorCode.Check;
            _sendingBuffer+= @"\pid\"+_queryResult["profileid"]+@"\final\";
            session.SendAsync(_sendingBuffer);         
        }

        private static void CheckDatabaseResult()
        {
            if (_queryResult == null)
            {
                _errorCode = GPErrorCode.DatabaseError;
            }
        }

        private static void IsContainAllKey()
        {
            if (!_recv.ContainsKey("nick") || !_recv.ContainsKey("email") || !_recv.ContainsKey("passenc"))
            {
                _errorCode = GPErrorCode.Parse;
                _errorMsg = "Parsing error";
            }
            if (!GameSpyUtils.IsEmailFormatCorrect(_recv["email"]))
            {
                _errorCode = GPErrorCode.Parse;
                _errorMsg = "Email format incorrect";
            }
        }
    }
}
