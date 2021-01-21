using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result
{
    internal sealed class KICKResult : ChatResultBase
    {
        public string ChannelName { get; set; }
        public string KickerNickName { get; set; }
        public string KickeeNickName { get; set; }
        public string KickerIRCPrefix { get; set; }
        public KICKResult()
        {
        }
    }
}
