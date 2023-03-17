using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Application;
using UniSpy.Server.PresenceConnectionManager.Contract.Request;
using UniSpy.Server.PresenceConnectionManager.Contract.Response;
using UniSpy.Server.PresenceConnectionManager.Contract.Result;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.Buddy
{
    /// <summary>
    /// TODO Status info should be stored in redis
    /// </summary>

    public sealed class StatusInfoHandler : LoggedInCmdHandlerBase
    {
        private new StatusInfoRequest _request => (StatusInfoRequest)base._request;
        private new StatusInfoResult _result { get => (StatusInfoResult)base._result; set => base._result = value; }

        public StatusInfoHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new StatusInfoResult();
        }

        protected override void DataOperation()
        {
            if (_request.IsGetStatusInfo)
            {
                var result = ClientManager.GetClient(_request.ProfileId, null, _request.NamespaceID);

                if (result is not null)
                {
                    // user is not online we do not need to send status info
                    _result.StatusInfo = result.Info.StatusInfo;
                }
            }
            else
            {
                _client.Info.StatusInfo = _request.StatusInfo;
                // TODO notify every online friend?
            }
        }

        protected override void ResponseConstruct()
        {
            if (_request.IsGetStatusInfo)
            {
                _response = new StatusInfoResponse(_request, _result);
            }
            else
            {
                base.ResponseConstruct();
            }
        }
    }
}
