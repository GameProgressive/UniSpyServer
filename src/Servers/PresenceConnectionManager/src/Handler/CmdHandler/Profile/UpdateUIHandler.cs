using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler
{
    /// <summary>
    /// Update user information (email)
    /// </summary>
    [HandlerContract("updateui")]
    public sealed class UpdateUIHandler : Abstraction.BaseClass.CmdHandlerBase
    {
        public UpdateUIHandler(IClient client, IRequest request) : base(client, request)
        {
            //todo find what data is belong to user info
            throw new System.NotImplementedException();
        }

        protected override void DataOperation()
        {
            throw new System.NotImplementedException();
        }
    }
}
