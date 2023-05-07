using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Contract.Request;
using UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.Buddy;
using UniSpy.Server.PresenceConnectionManager.Application;

namespace UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.General
{

    public sealed class SdkRevisionHandler : CmdHandlerBase
    {
        private new LoginRequest _request => (LoginRequest)base._request;
        public SdkRevisionHandler(Client client, LoginRequest request) : base(client, request)
        {
        }
        protected override void RequestCheck() { }
        protected override void DataOperation()
        {
            if (_client.Info.SdkRevision.IsSupportGPINewListRetrevalOnLogin)
            {
                //send buddy list and block list
                new BuddyListHandler(_client).Handle();
                new BlockListHandler(_client).Handle();
            }
        }
    }
}
