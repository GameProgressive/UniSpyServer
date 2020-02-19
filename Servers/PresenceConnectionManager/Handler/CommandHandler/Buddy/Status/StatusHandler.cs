using PresenceConnectionManager.Enumerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler.CommandHandler.Buddy.Status
{
    public class StatusHandler : GPCMHandlerBase
    {
        private uint _statusCode;
        public StatusHandler(Dictionary<string, string> recv) : base(recv)
        {
        }

        protected override void CheckRequest(GPCMSession session)
        {
            base.CheckRequest(session);
            if (_recv.ContainsKey("status"))
            {
                if (!uint.TryParse(_recv["status"], out _statusCode))
                {
                    _errorCode = GPErrorCode.Parse;
                }
            }
            else
                _errorCode = GPErrorCode.Parse;
            
               
        }

        protected override void DataBaseOperation(GPCMSession session)
        {
            session.UserInfo.StatusCode = (GPStatus)_statusCode;
            session.UserInfo.StatusString = _recv["statstring"];
            session.UserInfo.LocationString = _recv["locstring"];
        }
    }
}
