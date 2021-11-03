using UniSpyServer.Servers.Chat.Abstraction.BaseClass;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Result.Channel
{
    public sealed class SetChannelKeyResult : ResultBase
    {
        public string ChannelUserIRCPrefix { get; set; }
        public string ChannelName { get; set; }
        public SetChannelKeyResult()
        {
        }
    }
}