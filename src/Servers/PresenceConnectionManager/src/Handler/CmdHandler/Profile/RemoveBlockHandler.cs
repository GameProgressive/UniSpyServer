using System;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler
{
    [HandlerContract("removeblk")]
    public sealed class RemoveBlockHandler : Abstraction.BaseClass.CmdHandlerBase
    {
        public RemoveBlockHandler(IClient client, IRequest request) : base(client, request)
        {
            throw new NotImplementedException();
        }

        protected override void DataOperation()
        {
            throw new NotImplementedException();
        }
    }
}
