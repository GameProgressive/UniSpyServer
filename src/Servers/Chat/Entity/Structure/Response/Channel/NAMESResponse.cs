using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.Channel
{
    internal sealed class NAMESResponse : ChatChannelResponseBase
    {
        private new NAMESResult _result => (NAMESResult)base._result;
        public NAMESResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        protected override void BuildNormalResponse()
        {
            SendingBuffer = BuildNameReply(
                _result.RequesterNickName,
                _result.ChannelName,
                _result.ChannelUserNickString);
            SendingBuffer += BuildEndOfNameReply(
                _result.RequesterNickName,
                _result.ChannelName);
        }
        public static string BuildNameReply(string nickName, string channelName, string nicksString)
        {
            return ChatIRCReplyBuilder.Build(
                    ChatReplyName.NameReply,
                    $"{nickName} = {channelName}", nicksString);
        }
        public static string BuildEndOfNameReply(string nickName, string channelName)
        {
            string cmdParams = $"{nickName} {channelName}";
            string tailing = @"End of /NAMES list.";
            return ChatIRCReplyBuilder.Build(
                ChatReplyName.EndOfNames,
                cmdParams,
                tailing);
        }
    }
}
