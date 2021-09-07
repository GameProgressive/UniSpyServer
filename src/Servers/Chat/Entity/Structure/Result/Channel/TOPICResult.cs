using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result.Channel
{
    internal sealed class TOPICResult : ResultBase
    {
        public string ChannelName { get; set; }
        public string ChannelTopic { get; set; }

        public TOPICResult()
        {
        }
    }
}
