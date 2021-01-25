using System.Collections.Generic;
using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc.ChannelInfo;

namespace Chat.Entity.Structure.Result
{
    internal sealed class JOINResult : ChatResultBase
    {
        public string JoinerPrefix { get; set; }
        public string JoinerNickName{get;set;}
        public string ChannelUserNicks{get;set;}
        public bool IsCreateChannel { get; set; }
        public string ChannelModes { get; set; }
        public bool IsAlreadyJoinedChannel { get; set; }

        // public string ChannelModes { get; set; }
        // public List<string> NickNames { get; set; }
        public JOINResult()
        {
        }
    }
}
