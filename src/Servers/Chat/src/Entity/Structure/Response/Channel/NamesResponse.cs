using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Channel;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Response.Channel
{
    public sealed class NamesResponse : ChannelResponseBase
    {
        private new NamesResult _result => (NamesResult)base._result;
        public NamesResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
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
