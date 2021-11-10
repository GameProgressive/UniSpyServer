using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using System;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler
{
    [HandlerContract("removeblk")]
    public sealed class RemoveBlockHandler : Abstraction.BaseClass.CmdHandlerBase
    {
        public RemoveBlockHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            throw new NotImplementedException();
        }

        protected override void DataOperation()
        {
            throw new NotImplementedException();
        }
    }
}
