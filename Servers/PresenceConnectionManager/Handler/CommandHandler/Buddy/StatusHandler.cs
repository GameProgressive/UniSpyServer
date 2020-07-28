using GameSpyLib.Common.Entity.Interface;
using PresenceConnectionManager.Entity.Enumerator;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.CommandHandler.Buddy
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
            _session.UserData.UserStatus = (GPStatus)_statusCode;
            _session.UserData.StatusString = _recv["statstring"];
            _session.UserData.LocationString = _recv["locstring"];
        }
    }
}
