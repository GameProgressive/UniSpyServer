﻿using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc.ChannelInfo;

namespace Chat.Entity.Structure.Result
{
    internal sealed class GETCHANKEYResult : ChatResultBase
    {
        public string ChannelUserIRCPrefix { get; set; }
        public string ChannelName { get; set; }
        public string Values { get; set; }
        public GETCHANKEYResult()
        {
        }
    }
}