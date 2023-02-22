using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Result.Channel;

namespace UniSpy.Server.Chat.Contract.Response.Channel
{
    public sealed class NamesResponse : ChannelResponseBase
    {
        private new NamesResult _result => (NamesResult)base._result;
        public NamesResponse(RequestBase request, ResultBase result) : base(request, result){ }
        public override void Build()
        {
            SendingBuffer = BuildNameReply(
                _result.RequesterNickName,
                _result.ChannelName,
                _result.AllChannelUserNick);
            SendingBuffer += BuildEndOfNameReply(
                _result.RequesterNickName,
                _result.ChannelName);
        }
        public static string BuildNameReply(string nickName, string channelName, string nicksString)
        {
            return IRCReplyBuilder.Build(
                ResponseName.NameReply,
                $"{nickName} = {channelName}", nicksString);
        }
        public static string BuildEndOfNameReply(string nickName, string channelName)
        {
            string cmdParams = $"{nickName} {channelName}";
            string tailing = @"End of /NAMES list.";
            return IRCReplyBuilder.Build(
                ResponseName.EndOfNames,
                cmdParams,
                tailing);
        }
    }
}
