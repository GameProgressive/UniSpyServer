using UniSpyServer.Servers.Chat.Abstraction.BaseClass;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Result.Channel
{
    public sealed class KickResult : ResultBase
    {
        public string ChannelName { get; set; }
        public string KickerNickName { get; set; }
        public string KickeeNickName { get; set; }
        public string KickerIRCPrefix { get; set; }
        public KickResult()
        {
        }
    }
}
