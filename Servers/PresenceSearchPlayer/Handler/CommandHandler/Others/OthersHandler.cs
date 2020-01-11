using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.CommandHandler.Others
{
    /// <summary>
    /// Get buddy's information
    /// </summary>
    public class OthersHandler : GPSPHandlerBase
    {
        public OthersHandler(Dictionary<string, string> recv) : base(recv)
        {
        }

        protected override void CheckRequest(GPSPSession session)
        {
            if (!_recv.ContainsKey("profileid") || !_recv.ContainsKey("namespaceid"))
            {
                _errorCode = GPErrorCode.Parse;
            }
            base.CheckRequest(session);
        }

        protected override void DataBaseOperation(GPSPSession session)
        {
             _result = OthersQuery.GetOtherBuddy(Convert.ToUInt16(_recv["profileid"]), Convert.ToUInt16(_recv["namespaceid"]));
        }

        protected override void ConstructResponse(GPSPSession session)
        {
            if (_result.Count == 0)
            {
                _sendingBuffer = @"\others\\odone\final\";
                return;
            }
            _sendingBuffer = @"\others\";
            foreach (Dictionary<string, object> player in _result)
            {
                _sendingBuffer += @"\o\" + _recv["profileid"];
                _sendingBuffer += @"\nick\" + player["nick"];
                _sendingBuffer += @"\uniquenick\" + player["uniquenick"];
                _sendingBuffer += @"\first\" + player["firstname"];
                _sendingBuffer += @"\last\" + player["lastname"];
                _sendingBuffer += @"\email\" + player["email"];
            }
            _sendingBuffer += @"\odone\final\";
        }
    }
}
