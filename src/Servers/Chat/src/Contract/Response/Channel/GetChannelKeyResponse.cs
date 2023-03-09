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
        public GetChannelKeyResponse(GetChannelKeyRequest request, GetChannelKeyResult result) : base(request, result) { }

        public override void Build()
        {
            SendingBuffer = $":{_result.ChannelUserIRCPrefix} {ResponseName.GetChanKey} * {_result.ChannelName} {_request.Cookie} {_result.Values}\r\n";
        }
    }
}
