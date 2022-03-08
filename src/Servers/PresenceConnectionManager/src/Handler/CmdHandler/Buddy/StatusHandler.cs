using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler
{
    /// <summary>
    /// TODO Status should be stored in redis
    /// </summary>
    [HandlerContract("status")]
    public sealed class StatusHandler : CmdHandlerBase
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
