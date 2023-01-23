using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass
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
