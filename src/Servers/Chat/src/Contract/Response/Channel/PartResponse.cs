using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;

namespace UniSpy.Server.Chat.Contract.Response.Channel
{
    public sealed class PartResponse : ResponseBase
    {
        private new PartRequest _request => (PartRequest)base._request;
        private new PartResult _result => (PartResult)base._result;
        public PartResponse(PartRequest request, PartResult result) : base(request, result) { }

        public override void Build()
        {
            SendingBuffer = BuildPartReply(
                _result.LeaverIRCPrefix,
                _result.ChannelName,
                _request.Reason);
        }

        public static string BuildPartReply(string userIRCPrefix, string channelName, string message)
        {
            return IRCReplyBuilder.Build(
                userIRCPrefix,
                ResponseName.Part,
                channelName,
                message);
        }
    }
}
