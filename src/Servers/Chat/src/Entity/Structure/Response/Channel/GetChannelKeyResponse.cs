using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Channel;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Response.Channel
{
    public sealed class GetChannelKeyResponse : ChannelResponseBase
    {
        private new GetChannelKeyResult _result => (GetChannelKeyResult)base._result;
        private new GetChannelKeyRequest _request => (GetChannelKeyRequest)base._request;
        public GetChannelKeyResponse(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
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
