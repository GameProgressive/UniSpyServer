using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceConnectionManager.Entity.Structure.Request;
using PresenceConnectionManager.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    internal class StatusHandler : PCMCmdHandlerBase
    {
        protected new StatusRequest _request => (StatusRequest)base._request;
        protected new StatusResult _result
        {
            get => (StatusResult)base._result;
            set => base._result = value;
        }

        public StatusHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new StatusResult();
        }

        protected override void DataOperation()
        {
            //TODO check if statushandler need send response
            //if (_request == null)
            //{
            //    // get user status
            //    _result.UserStatus = _session.UserInfo.UserCurrentStatus;
            //    _result.StatusString = _session.UserInfo.StatusString;
            //    _result.LocationString = _session.UserInfo.LocationString;
            //}
            //else
            //{
                // set user status
                _session.UserInfo.UserCurrentStatus = _request.StatusCode;
                _session.UserInfo.StatusString = _request.StatusString;
                _session.UserInfo.LocationString = _request.LocationString;
            //}
        }
    }
}
