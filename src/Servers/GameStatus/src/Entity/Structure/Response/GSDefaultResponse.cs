﻿using UniSpyServer.GameStatus.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.GameStatus.Entity.Structure.Response
{
    public sealed class GSDefaultResponse : ResponseBase
    {
        public GSDefaultResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
        }
    }
}
