﻿using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request
{
    public class UTMRequest : ChatMsgRequestBase
    {
        public UTMRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}