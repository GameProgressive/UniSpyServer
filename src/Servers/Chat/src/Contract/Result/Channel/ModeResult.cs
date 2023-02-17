using UniSpy.Server.Chat.Abstraction.BaseClass;

namespace UniSpy.Server.Chat.Contract.Result.Channel
{
    public sealed class ModeResult : ResultBase
    {
        public string ChannelModes { get; set; }
        public string ChannelName { get; set; }
        public string JoinerNickName { get; set; }
        public ModeResult(){ }
    }
}
