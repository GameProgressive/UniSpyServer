using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;
using PresenceSearchPlayer.Handler.CommandHandler.Error;

namespace PresenceSearchPlayer.Handler.CommandHandler.Check
{
    public class CheckHandler:GPSPHandlerBase
    {
        // \check\\nick\<nick>\email\<email>\partnerid\0\passenc\<passenc>\gamename\gmtest\final\
        //\cur\pid\<pid>\final
        //check is request recieved correct and convert password into our MD5 type
        public CheckHandler(Dictionary<string, string> recv) : base(recv)
        {
        }
        protected override void CheckRequest(GPSPSession session)
        {
            if (!_recv.ContainsKey("nick") || !_recv.ContainsKey("email") || !_recv.ContainsKey("passenc"))
            {
                _errorCode = GPErrorCode.Parse;
            }
            if (!GameSpyUtils.IsEmailFormatCorrect(_recv["email"]))
            {
                _errorCode = GPErrorCode.CheckBadMail;
            }
        }

        protected override void DataBaseOperation(GPSPSession session)
        {
            if (!CheckQuery.FindEmail(_recv["email"]))
            {
                _errorCode = GPErrorCode.CheckBadMail;
                return;
            }
            if (!CheckQuery.CheckPassword(_recv["email"], _recv["passenc"]))
            {
                _errorCode = GPErrorCode.CheckBadPassword;
                return;
            }

            if (!CheckQuery.FindNick(_recv["nick"]))
            {
                _errorCode = GPErrorCode.CheckBadNick;
                return;
            }

            _result = CheckQuery.GetProfileidFromNickEmailPassword(_recv["email"], _recv["passenc"], _recv["nick"]);
            if (_result== null)
            {
                _errorCode = GPErrorCode.CheckBadNick;
            }
        }

        protected override void ConstructResponse(GPSPSession session)
        {
            if (_errorCode != GPErrorCode.NoError)
            {
                _sendingBuffer = @"\cur\" + (uint)_errorCode + @"\final\";
            }
            else
            {
                _sendingBuffer = @"\cur\0\pid\" + _result[0]["profileid"] + @"\final\";
            }
        }
    }
}
