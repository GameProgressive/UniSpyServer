using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Contract.Request;
using UniSpy.Server.PresenceConnectionManager.Contract.Result;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.Buddy
{
    /// <summary>
    /// TODO Status should be stored in redis
    /// </summary>

    public sealed class StatusHandler : LoggedInCmdHandlerBase
    {
        private new StatusRequest _request => (StatusRequest)base._request;
        private new StatusResult _result { get => (StatusResult)base._result; set => base._result = value; }

        public StatusHandler(IClient client, IRequest request) : base(client, request)
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
                _client.Info.Status.CurrentStatus = _request.Status.CurrentStatus;
                _client.Info.Status.StatusString = _request.Status.StatusString;
                _client.Info.Status.LocationString = _request.Status.LocationString;
            }
        }
    }
}
