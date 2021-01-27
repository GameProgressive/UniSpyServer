using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    internal sealed class GETCKEYResponse : ChatResponseBase
    {
        private new GETCKEYResult _result => (GETCKEYResult)base._result;
        private new GETCKEYRequest _request => (GETCKEYRequest)base._request;

        public GETCKEYResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public static string BuildGetCKeyReply(string nickname, string channelName, string cookie, string flags)
        {
            var cmdParams = $"* {channelName} {nickname} {cookie} {flags}";
            return ChatIRCReplyBuilder.Build(ChatReplyName.GetCKey, cmdParams);
        }

        public static string BuildEndOfGetCKeyReply(string channelName, string cookie)
        {
            var cmdParams = $"param1 {channelName} {cookie}";
            var tailing = "End Of /GETCKEY.";
            return ChatIRCReplyBuilder.Build(
                ChatReplyName.EndGetCKey,
                cmdParams,
                tailing);
        }
        protected override void BuildNormalResponse()
        {
            SendingBuffer = "";
            foreach (var data in _result.DataResults)
            {
                SendingBuffer += ChatIRCReplyBuilder.Build(ChatReplyName.GetCKey,
                $"* {data.NickName} {_result.ChannelName} {_request.Cookie} {data.UserValues}");
            }

            SendingBuffer += ChatIRCReplyBuilder.Build(ChatReplyName.EndGetCKey,
                 $"* {_result.ChannelName} {_request.Cookie}",
                 "End Of /GETCKEY.");
        }
    }
}
