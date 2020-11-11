using UniSpyLib.Abstraction.Interface;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceConnectionManager.Entity.Structure.Request.Buddy;
using System.Collections.Generic;

namespace PresenceConnectionManager.Abstraction.BaseClass.Buddy
{
    public class StatusHandler : PCMCommandHandlerBase
    {
        protected new StatusRequest _request;
        public StatusHandler(ISession session, IRequest request) : base(session, request)
        {
            _request = (StatusRequest)request;
        }

        protected override void DataOperation()
        {
            _session.UserData.UserStatus = (GPStatus)_request.StatusCode;
            _session.UserData.StatusString = _request.StatusString;
            _session.UserData.LocationString = _request.LocationString;
        }
    }
}
