using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler
{
    [HandlerContract("sdk")]
    public sealed class SDKRevisionHandler : CmdHandlerBase
    {
        private new LoginRequest _request => (LoginRequest)base._request;
        public SDKRevisionHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            if (_session.UserInfo.SDKRevision.IsSupportGPINewListRetrevalOnLogin)
            {
                //    //send buddy list and block list
                new BuddyListHandler(_session, null).Handle();
                new BlockListHandler(_session, null).Handle();
            }
        }
    }
}
