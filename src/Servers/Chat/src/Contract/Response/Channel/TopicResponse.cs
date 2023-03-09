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

        public override void Build()
        {
            if (_result.ChannelTopic == "" || _result.ChannelTopic is null)
            {
                SendingBuffer = $":{ServerDomain} {ResponseName.NoTopic} {_result.ChannelName}\r\n";
            }
            else
            {
                SendingBuffer = $":{ServerDomain} {ResponseName.NoTopic} {_result.ChannelName} :{_result.ChannelTopic}\r\n";
            }
        }
    }
}
