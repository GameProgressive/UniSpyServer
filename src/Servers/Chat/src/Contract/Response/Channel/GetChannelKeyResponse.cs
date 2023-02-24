using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;

namespace UniSpy.Server.Chat.Contract.Response.Channel
{
    public sealed class GetChannelKeyResponse : ChannelResponseBase
    {
        private new GetChannelKeyRequest _request => (GetChannelKeyRequest)base._request;
        private new GetChannelKeyResult _result => (GetChannelKeyResult)base._result;
        public GetChannelKeyResponse(GetChannelKeyRequest request, GetChannelKeyResult result) : base(request, result){ }

        public override void Build()
        {
            var cmdParams = $"param1 {_result.ChannelName} {_request.Cookie} {_result.Values}";
            SendingBuffer = IRCReplyBuilder.Build(
                _result.ChannelUserIRCPrefix,
                ResponseName.GetChanKey,
                cmdParams,
                null);
        }
    }
}
