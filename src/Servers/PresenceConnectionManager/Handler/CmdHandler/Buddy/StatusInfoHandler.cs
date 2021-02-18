using System.Linq;
using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Application;
using PresenceConnectionManager.Entity.Structure.Request;
using PresenceConnectionManager.Entity.Structure.Response.Buddy;
using PresenceConnectionManager.Entity.Structure.Result.Buddy;
using PresenceConnectionManager.Network;
using UniSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    /// <summary>
    /// TODO Status info should be stored in redis
    /// </summary>
    internal class StatusInfoHandler : PCMCmdHandlerBase
    {
        protected new StatusInfoRequest _request => (StatusInfoRequest)base._request;
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
                var result = (PCMSession)PCMServerFactory.Server.SessionManager.Sessions.Values
                                .Where(session => ((PCMSession)session).UserInfo.BasicInfo.ProfileID == _request.ProfileID
                                && ((PCMSession)session).UserInfo.BasicInfo.NamespaceID == _session.UserInfo.BasicInfo.NamespaceID)
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
