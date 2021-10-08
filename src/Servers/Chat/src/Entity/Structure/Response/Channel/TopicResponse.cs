using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.Channel
{
    internal sealed class TopicResponse : ResponseBase
    {
        private new TopicResult _result => (TopicResult)base._result;
        public TopicResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

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
            if (_result.ChannelTopic == "" || _result.ChannelTopic == null)
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
