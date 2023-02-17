using UniSpy.Server.Chat.Abstraction.BaseClass;

namespace UniSpy.Server.Chat.Contract.Result.Channel
{
    public sealed class JoinResult : ResultBase
    {
        public string JoinerPrefix { get; set; }
        public string JoinerNickName { get; set; }
        public string AllChannelUserNicks { get; set; }
        public string ChannelModes { get; set; }

        // public string ChannelModes { get; set; }
        // public List<string> NickNames { get; set; }
        public JoinResult(){ }
    }
}
