﻿using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.CDkey.Abstraction.BaseClass
{
    public abstract class RequestBase : UniSpyLib.Abstraction.BaseClass.RequestBase
    {
        public RequestBase(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
        }
    }
}
