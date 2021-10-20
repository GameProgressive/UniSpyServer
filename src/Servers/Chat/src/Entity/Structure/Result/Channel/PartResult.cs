using UniSpyServer.Chat.Abstraction.BaseClass;

namespace UniSpyServer.Chat.Entity.Structure.Result.Channel
{
    public sealed class PartResult : ResultBase
    {
        public string LeaverIRCPrefix { get; set; }
        public bool IsChannelCreator { get; set; }
        public string ChannelName { get; set; }
        public string Message { get; set; }
        public PartResult()
        {
        }
    }
}
