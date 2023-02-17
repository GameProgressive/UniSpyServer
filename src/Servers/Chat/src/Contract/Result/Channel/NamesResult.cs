using UniSpy.Server.Chat.Abstraction.BaseClass;

namespace UniSpy.Server.Chat.Contract.Result.Channel
{
    public sealed class NamesResult : ResultBase
    {
        public string AllChannelUserNick { get; set; }
        public string ChannelName { get; set; }
        public string RequesterNickName { get; set; }
        public NamesResult(){ }
    }
}
