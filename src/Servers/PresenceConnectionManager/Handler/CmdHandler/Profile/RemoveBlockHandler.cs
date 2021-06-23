using PresenceConnectionManager.Abstraction.BaseClass;
using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    [Command("removeblk")]
    internal sealed class RemoveBlockHandler : PCMCmdHandlerBase
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
