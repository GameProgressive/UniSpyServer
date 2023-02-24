using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;

namespace UniSpy.Server.Chat.Contract.Response.Channel
{
    public sealed class GetCKeyResponse : ResponseBase
    {
        private new GetCKeyRequest _request => (GetCKeyRequest)base._request;
        private new GetCKeyResult _result => (GetCKeyResult)base._result;

        public GetCKeyResponse(GetCKeyRequest request, GetCKeyResult result) : base(request, result){ }
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
                $"* {data.NickName} {_request.ChannelName} {_request.Cookie} {data.UserValues}");
            }

            SendingBuffer += IRCReplyBuilder.Build(ResponseName.EndGetCKey,
                 $"* {_request.ChannelName} {_request.Cookie}",
                 "End Of /GETCKEY.");
        }
    }
}
