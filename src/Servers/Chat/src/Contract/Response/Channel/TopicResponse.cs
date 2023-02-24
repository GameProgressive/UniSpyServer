using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;

namespace UniSpy.Server.Chat.Contract.Response.Channel
{
    public sealed class TopicResponse : ResponseBase
    {
        private new TopicRequest _request => (TopicRequest)base._request;
        private new TopicResult _result => (TopicResult)base._result;
        public TopicResponse(TopicRequest request, TopicResult result) : base(request, result) { }

        public static string BuildNoTopicReply(string channelName)
        {
            return IRCReplyBuilder.Build(ResponseName.NoTopic, channelName);
        }
        public static string BuildTopicReply(string channelName, string channelTopic)
        {
            return IRCReplyBuilder.Build(ResponseName.ChannelTopic, channelName, channelTopic);
        }

        public override void Build()
        {
            if (_result.ChannelTopic == "" || _result.ChannelTopic is null)
            {
                SendingBuffer =
                    TopicResponse.BuildNoTopicReply(
                    _result.ChannelName);
            }
            else
            {
                SendingBuffer =
                    TopicResponse.BuildTopicReply(
                    _result.ChannelName,
                    _result.ChannelTopic);
            }
        }
    }
}
