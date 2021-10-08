using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Application;
using PresenceConnectionManager.Entity.Contract;
using PresenceConnectionManager.Entity.Structure.Request;
using PresenceConnectionManager.Entity.Structure.Response.Buddy;
using PresenceConnectionManager.Entity.Structure.Result.Buddy;
using PresenceConnectionManager.Network;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    /// <summary>
    /// TODO Status info should be stored in redis
    /// </summary>
    [HandlerContract("statusinfo")]
    internal sealed class StatusInfoHandler : Abstraction.BaseClass.CmdHandlerBase
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
