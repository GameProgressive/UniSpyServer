using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Structure.Misc;
using UniSpyServer.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Chat.Entity.Structure.Result.Channel;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Chat.Entity.Structure.Response.Channel
{
    public sealed class GetCKeyResponse : ResponseBase
    {
        private new GetCKeyResult _result => (GetCKeyResult)base._result;
        private new GetCKeyRequest _request => (GetCKeyRequest)base._request;

        public GetCKeyResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public static string BuildGetCKeyReply(string nickname, string channelName, string cookie, string flags)
        {
            var cmdParams = $"* {channelName} {nickname} {cookie} {flags}";
            return IRCReplyBuilder.Build(ResponseName.GetCKey, cmdParams);
        }

        public static string BuildEndOfGetCKeyReply(string channelName, string cookie)
        {
            var cmdParams = $"param1 {channelName} {cookie}";
            var tailing = "End Of /GETCKEY.";
            return IRCReplyBuilder.Build(
                ResponseName.EndGetCKey,
                cmdParams,
                tailing);
        }
        public override void Build()
        {
            SendingBuffer = "";
            foreach (var data in _result.DataResults)
            {
                SendingBuffer += IRCReplyBuilder.Build(ResponseName.GetCKey,
                $"* {data.NickName} {_result.ChannelName} {_request.Cookie} {data.UserValues}");
            }

            SendingBuffer += IRCReplyBuilder.Build(ResponseName.EndGetCKey,
                 $"* {_result.ChannelName} {_request.Cookie}",
                 "End Of /GETCKEY.");
        }
    }
}
