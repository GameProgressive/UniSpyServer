﻿using PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Response
{
    internal class KeepAliveResponse : PCMResponseBase
    {
        public KeepAliveResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
            SendingBuffer = @"\ka\\final\";
        }
    }
}