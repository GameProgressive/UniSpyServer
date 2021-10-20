using UniSpyServer.Chat.Abstraction.BaseClass;

namespace UniSpyServer.Chat.Entity.Structure.Result.Channel
{
    public sealed class TopicResult : ResultBase
    {
        public string ChannelName { get; set; }
        public string ChannelTopic { get; set; }

        public TopicResult()
        {
        }
    }
}
