﻿using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.CDkey.Abstraction.BaseClass
{
    public abstract class RequestBase : UniSpyRequestBase
    {
        public RequestBase(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
        }
    }
}
