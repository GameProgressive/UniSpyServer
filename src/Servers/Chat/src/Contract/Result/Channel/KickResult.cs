using UniSpy.Server.Chat.Abstraction.BaseClass;

namespace UniSpy.Server.Chat.Contract.Result.Channel
{
    public sealed class KickResult : ResultBase
    {
        public string ChannelName { get; set; }
        public string KickerNickName { get; set; }
        public string KickeeNickName { get; set; }
        public string KickerIRCPrefix { get; set; }
        public KickResult(){ }
    }
}
