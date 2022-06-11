using System.Linq;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result;
using UniSpyServer.Servers.PresenceConnectionManager.Structure.Data;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler.Buddy
{
    /// <summary>
    /// TODO Status info should be stored in redis
    /// </summary>

    public sealed class StatusInfoHandler : Abstraction.BaseClass.CmdHandlerBase
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
                var result = (Client)Client.ClientPool.Values.FirstOrDefault(s => ((ClientInfo)s.Info).SubProfileInfo.ProfileId == _request.ProfileId
                && ((ClientInfo)s.Info).SubProfileInfo.NamespaceId == ((ClientInfo)s.Info).SubProfileInfo.NamespaceId);

                if (result != null)
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
