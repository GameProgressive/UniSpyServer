using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result.Channel
{
    public sealed class SetChannelKeyResult : ResultBase
    {
        public string ChannelUserIRCPrefix { get; set; }
        public string ChannelName { get; set; }
        public SetChannelKeyResult()
        {
        }
    }
}