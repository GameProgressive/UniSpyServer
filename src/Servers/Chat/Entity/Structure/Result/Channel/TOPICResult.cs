using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result.Channel
{
    internal sealed class TOPICResult : ChatResultBase
    {
        public string ChannelName { get; set; }
        public string ChannelTopic { get; set; }

        public TOPICResult()
        {
        }
    }
}
