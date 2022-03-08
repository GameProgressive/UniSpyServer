using UniSpyServer.Servers.Chat.Abstraction.BaseClass;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Result.Channel
{
    public sealed class PartResult : ResultBase
    {
        public string LeaverIRCPrefix { get; set; }
        public bool IsChannelCreator { get; set; }
        public string ChannelName { get; set; }
        public PartResult() { }
    }
}
