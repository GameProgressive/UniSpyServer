using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceConnectionManager.Entity.Structure.Request;
using UniSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    internal class StatusHandler : PCMCmdHandlerBase
    {
        protected new StatusRequest _request => (StatusRequest)base._request;

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
