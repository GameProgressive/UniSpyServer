using UniSpy.Server.PresenceSearchPlayer.Exception.General;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.PresenceConnectionManager.Aggregate.Redis;

namespace UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass
{
    public abstract class LoggedInCmdHandlerBase : CmdHandlerBase
    {
        public LoggedInCmdHandlerBase(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void RequestCheck()
        {
            if (_client.Info.LoginStat != Enumerate.LoginStatus.Completed)
            {
                throw new GPException("You are not logged in, please login first.");
            }
            base.RequestCheck();
        }
        public override void Handle()
        {
            base.Handle();
            try
            {
                // we publish this message to redis channel
                PublishMessage();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        protected virtual void PublishMessage()
        {
            // we do not publish message when the message is received from remote client
            if (_client.Info.IsRemoteClient)
            {
                return;
            }
            var msg = new UserInfoMessage(_client.GetRemoteClient());
            Application.Server.UserInfoChannel.PublishMessage(msg);
        }
    }
}
