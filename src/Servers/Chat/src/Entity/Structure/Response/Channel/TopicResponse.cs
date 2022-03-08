using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Channel;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Response.Channel
{
    public sealed class TopicResponse : ResponseBase
    {
        private new TopicResult _result => (TopicResult)base._result;
        public TopicResponse(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result){ }

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
