using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.Channel
{
    internal sealed class TOPICResponse : ChatResponseBase
    {
        private new TOPICResult _result => (TOPICResult)base._result;
        public TOPICResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public static string BuildNoTopicReply(string channelName)
        {
            return ChatIRCReplyBuilder.Build(ChatReplyName.NoTopic, channelName);
        }
        public static string BuildTopicReply(string channelName, string channelTopic)
        {
            return ChatIRCReplyBuilder.Build(ChatReplyName.TOPIC, channelName, channelTopic);
        }

        protected override void BuildNormalResponse()
        {
            if (_result.ChannelTopic == "" || _result.ChannelTopic == null)
            {
                SendingBuffer =
                    TOPICResponse.BuildNoTopicReply(
                    _result.ChannelName);
            }
            else
            {
                SendingBuffer =
                    TOPICResponse.BuildTopicReply(
                   _result.ChannelName,
                   _result.ChannelTopic);
            }
        }
    }
}
