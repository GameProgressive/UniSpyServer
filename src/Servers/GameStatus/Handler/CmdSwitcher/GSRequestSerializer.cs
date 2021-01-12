using System;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace GameStatus.Handler.CmdSwitcher
{
    internal class GSRequestSerializer:UniSpyRequestSerializerBase
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
