using GameSpyLib.Common.Entity.Interface;
using PresenceConnectionManager.Enumerator;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.CommandHandler.Buddy.Status
{
    public class StatusHandler : PCMCommandHandlerBase
    {
        private uint _statusCode;

        public StatusHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();

            if (_recv.ContainsKey("status"))
            {
                if (!uint.TryParse(_recv["status"], out _statusCode))
                {
                    _errorCode = GPErrorCode.Parse;
                }
            }
            else
            {
                _errorCode = GPErrorCode.Parse;
            }
        }

        protected override void DataOperation()
        {
            _session.UserInfo.StatusCode = (GPStatus)_statusCode;
            _session.UserInfo.StatusString = _recv["statstring"];
            _session.UserInfo.LocationString = _recv["locstring"];
        }
    }
}
