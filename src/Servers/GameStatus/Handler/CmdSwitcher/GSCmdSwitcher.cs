using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace GameStatus.Handler.CmdSwitcher
{
    public class GSCmdSwitcher : UniSpyCmdSwitcherBase
    {
        public GSCmdSwitcher(IUniSpySession session, object rawRequest) : base(session, rawRequest)
        {
        }

        protected override void SerializeCommandHandlers()
        {
            throw new NotImplementedException();
        }

        protected override void SerializeRequests()
        {
            throw new NotImplementedException();
        }
    }
}
