using System;
using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result.Channel
{
    internal sealed class SETCHANKEYResult : ChatResultBase
    {
        public string ChannelUserIRCPrefix { get; set; }
        public string ChannelName { get; set; }
        public SETCHANKEYResult()
        {
        }
    }
}