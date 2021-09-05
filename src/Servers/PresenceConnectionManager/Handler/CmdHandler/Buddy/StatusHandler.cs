using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Request;
using PresenceConnectionManager.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    /// <summary>
    /// TODO Status should be stored in redis
    /// </summary>
    internal sealed class StatusHandler : PCMCmdHandlerBase
    {
        [Command("status")]
        private new StatusRequest _request => (StatusRequest)base._request;
        private new StatusResult _result
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
            // set user status
            if (_request.IsGetStatus)
            {
                //TODO check if statushandler need send response
            }
            else
            {
                _session.UserInfo.Status.CurrentStatus = _request.Status.CurrentStatus;
                _session.UserInfo.Status.StatusString = _request.Status.StatusString;
                _session.UserInfo.Status.LocationString = _request.Status.LocationString;
            }
        }
    }
}
