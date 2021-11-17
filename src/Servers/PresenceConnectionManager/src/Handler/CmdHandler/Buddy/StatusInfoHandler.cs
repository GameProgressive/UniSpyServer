using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Application;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request.Buddy;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response.Buddy;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result.Buddy;
using UniSpyServer.Servers.PresenceConnectionManager.Network;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler
{
    /// <summary>
    /// TODO Status info should be stored in redis
    /// </summary>
    [HandlerContract("statusinfo")]
    public sealed class StatusInfoHandler : Abstraction.BaseClass.CmdHandlerBase
    {
        private new StatusInfoRequest _request => (StatusInfoRequest)base._request;
        private new StatusInfoResult _result
        {
            get => (StatusInfoResult)base._result;
            set => base._result = value;
        }

        public StatusInfoHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new StatusInfoResult();
        }

        protected override void DataOperation()
        {
            if (_request.IsGetStatusInfo)
            {
                var result = (Session)ServerFactory.Server.SessionManager.SessionPool.Values
                                .Where(session => ((Session)session).UserInfo.BasicInfo.ProfileID == _request.ProfileID
                                && ((Session)session).UserInfo.BasicInfo.NamespaceID == _session.UserInfo.BasicInfo.NamespaceID)
                                .FirstOrDefault();
                if (result != null)
                {
                    // user is not online we do not need to send status info
                    _result.StatusInfo = result.UserInfo.StatusInfo;
                }
            }
            else
            {
                _session.UserInfo.StatusInfo = _request.StatusInfo;
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
