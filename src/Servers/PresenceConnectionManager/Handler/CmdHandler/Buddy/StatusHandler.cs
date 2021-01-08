using UniSpyLib.Abstraction.Interface;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceConnectionManager.Entity.Structure.Request;
using System.Collections.Generic;
using PresenceConnectionManager.Abstraction.BaseClass;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    public class StatusHandler : PCMCmdHandlerBase
    {
        protected new StatusRequest _request
        {
            get { return (StatusRequest)base._request; }
        }

        public StatusHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            _session.UserInfo.UserStatus = (GPStatus)_request.StatusCode;
            _session.UserInfo.StatusString = _request.StatusString;
            _session.UserInfo.LocationString = _request.LocationString;
        }
    }
}
