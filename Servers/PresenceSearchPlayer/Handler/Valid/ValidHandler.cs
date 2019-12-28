using GameSpyLib.Common;
using GameSpyLib.Logging;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.Valid
{
    public class ValidHandler : GPSPHandlerBase
    {
        public ValidHandler(Dictionary<string, string> recv) : base(recv)
        {
        }
        bool EmailValid;
        
        protected override void CheckRequest(GPSPSession session)
        {
            if (!_recv.ContainsKey("email"))
            {
                _errorCode = GPErrorCode.Parse;
            }
            if (!GameSpyUtils.IsEmailFormatCorrect(_recv["email"]))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void ConstructResponse(GPSPSession session)
        {
            if(EmailValid)
            {
                session.Send(@"\vr\1\final\");
            }
            else
            {
                session.Send(@"\vr\0\final\");
            }
        }

        protected override void DataBaseOperation(GPSPSession session)
        {
            if (ValidQuery.IsEmailValid(_recv["email"]))
            {
                EmailValid = true;
            }
        }
    }
}
