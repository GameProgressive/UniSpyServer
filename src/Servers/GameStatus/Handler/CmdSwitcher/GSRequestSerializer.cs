using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace GameStatus.Handler.CmdSwitcher
{
    internal class GSRequestSerializer : UniSpyRequestFactoryBase
    {
        public GSRequestSerializer(object rawRequest) : base(rawRequest)
        {
        }

        public override IUniSpyRequest Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
