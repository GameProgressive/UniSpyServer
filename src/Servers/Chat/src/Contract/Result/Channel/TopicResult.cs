using UniSpy.Server.Chat.Abstraction.BaseClass;

namespace UniSpy.Server.Chat.Contract.Result.Channel
{
    public sealed class TopicResult : ResultBase
    {
        public string ChannelName { get; set; }
        public string ChannelTopic { get; set; }

        public TopicResult(){ }
    }
}
