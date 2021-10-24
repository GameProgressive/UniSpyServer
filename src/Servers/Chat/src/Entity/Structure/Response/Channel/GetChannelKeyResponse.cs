using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Structure.Misc;
using UniSpyServer.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Chat.Entity.Structure.Result.Channel;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Chat.Entity.Structure.Response.Channel
{
    public sealed class GetChannelKeyResponse : ChannelResponseBase
    {
        private new GetChannelKeyResult _result => (GetChannelKeyResult)base._result;
        private new GetChannelKeyRequest _request => (GetChannelKeyRequest)base._request;
        public GetChannelKeyResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

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
