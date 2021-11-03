using UniSpyServer.Servers.Chat.Abstraction.BaseClass;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Result.Channel
{
    public sealed class ModeResult : ResultBase
    {
        public string ChannelModes { get; set; }
        public string ChannelName { get; set; }
        public string JoinerNickName { get; set; }
        public ModeResult()
        {
        }
    }
}
