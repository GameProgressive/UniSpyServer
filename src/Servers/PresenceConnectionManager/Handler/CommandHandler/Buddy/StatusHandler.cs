using GameSpyLib.Abstraction.Interface;
using PresenceConnectionManager.Entity.Enumerator;
using PresenceConnectionManager.Entity.Structure.Request.Buddy;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.CommandHandler.Buddy
{
    public class StatusHandler : PCMCommandHandlerBase
    {
        protected StatusRequest _request;
        public StatusHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
            _request = new StatusRequest(recv);
        }

        protected override void CheckRequest()
        {
            _errorCode = _request.Parse();
        }

        protected override void DataOperation()
        {
            _session.UserData.UserStatus = (GPStatus)_request.StatusCode;
            _session.UserData.StatusString = _request.StatusString;
            _session.UserData.LocationString = _request.LocationString;
        }
    }
}
