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
    public class NickHandler : GPSPHandlerBase
    {
        public NickHandler(Dictionary<string, string> recv) : base(recv)
        {
        }

        protected override void CheckRequest(GPSPSession session)
        {
            if (!_recv.ContainsKey("email"))
            {
                _errorCode = GPErrorCode.Parse;

            }
            // First, we try to receive an encoded password
            if (!_recv.ContainsKey("passenc"))
            {
                // If the encoded password is not sended, we try receiving the password in plain text
                if (!_recv.ContainsKey("pass"))
                {
                    // No password is specified, we cannot continue                   
                    _errorCode = GPErrorCode.Parse;
                }
                if (!_recv.ContainsKey("namespaceid"))
                {
                    _recv.Add("namespaceid", "0");
                }
            }
            base.CheckRequest(session);
        }

        protected override void DataBaseOperation(GPSPSession session)
        {
            _result = NickQuery.RetriveNicknames(_recv["email"], _recv["passenc"], Convert.ToUInt16(_recv["namespaceid"]));
        }

        protected override void ConstructResponse(GPSPSession session)
        {
            if (_result == null)
            {
                _sendingBuffer = @"\nr\\ndone\final\";
            }
            else
            {
                _sendingBuffer = @"\nr\";
                foreach (Dictionary<string, object> player in _result)
                {
                    _sendingBuffer += @"\nick\";
                    _sendingBuffer += player["nick"];
                    _sendingBuffer += @"\uniquenick\";
                    _sendingBuffer += player["uniquenick"];
                }
                _sendingBuffer += @"\ndone\final\";
            }
           
        }
    }
}
