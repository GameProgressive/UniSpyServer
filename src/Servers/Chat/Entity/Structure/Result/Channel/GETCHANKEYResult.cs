using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc.ChannelInfo;

namespace Chat.Entity.Structure.Result
{
    internal sealed class GETCHANKEYResult : ChatResultBase
    {
        public ChatChannelUser ChannelUser { get; set; }
        public string ChannelName { get;  set; }
        public string Cookie { get;  set; }
        public string Flags { get;  set; }
        public GETCHANKEYResult()
        {
        }
    }
}
