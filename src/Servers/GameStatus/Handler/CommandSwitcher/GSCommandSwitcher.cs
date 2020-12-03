using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace GameStatus.Handler.CommandSwitcher
{
    public class GSCommandSwitcher : CommandSwitcherBase
    {
        public GSCommandSwitcher(ISession session, object rawRequest) : base(session, rawRequest)
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
