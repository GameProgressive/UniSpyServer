using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result.Channel
{
    internal sealed class JOINResult : ResultBase
    {
        public string JoinerPrefix { get; set; }
        public string JoinerNickName { get; set; }
        public string AllChannelUserNicks { get; set; }
        public string ChannelModes { get; set; }

        // public string ChannelModes { get; set; }
        // public List<string> NickNames { get; set; }
        public JOINResult()
        {
        }
    }
}
