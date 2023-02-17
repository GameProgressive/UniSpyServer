using UniSpy.Server.PresenceSearchPlayer.Entity.Exception.General;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass
{
    public abstract class LoggedInCmdHandlerBase : CmdHandlerBase
    {
        public LoggedInCmdHandlerBase(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void RequestCheck()
        {
            if(_client.Info.LoginStat!=Entity.Enumerate.LoginStatus.Completed)
            {
                throw new GPException("You are not logged in, please login first.");
            }
            base.RequestCheck();
        }
    }
}
