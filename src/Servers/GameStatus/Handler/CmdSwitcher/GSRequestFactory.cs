using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace GameStatus.Handler.CmdSwitcher
{
    internal class GSRequestFactory : UniSpyRequestFactoryBase
    {
        public GSRequestFactory(object rawRequest) : base(rawRequest)
        {
        }

        public override IUniSpyRequest Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
