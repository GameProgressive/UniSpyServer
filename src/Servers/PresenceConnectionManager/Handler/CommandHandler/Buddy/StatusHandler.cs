using UniSpyLib.Abstraction.Interface;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceConnectionManager.Entity.Structure.Request.Buddy;
using System.Collections.Generic;
using PresenceConnectionManager.Abstraction.BaseClass;

namespace PresenceConnectionManager.Handler.CommandHandler
{
    public class StatusHandler : PCMCommandHandlerBase
    {
        protected new StatusRequest _request { get { return (StatusRequest)base._request; } }
        public StatusHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            _session.UserData.UserStatus = (GPStatus)_request.StatusCode;
            _session.UserData.StatusString = _request.StatusString;
            _session.UserData.LocationString = _request.LocationString;
        }
    }
}
