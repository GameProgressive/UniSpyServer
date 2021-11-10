using UniSpyServer.Servers.Chat.Abstraction.BaseClass;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Result.Channel
{
    public sealed class GetChannelKeyResult : ResultBase
    {
        public string ChannelUserIRCPrefix { get; set; }
        public string ChannelName { get; set; }
        public string Values { get; set; }
        public GetChannelKeyResult()
        {
        }
    }
}
