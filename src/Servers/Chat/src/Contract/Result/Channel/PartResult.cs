using UniSpy.Server.Chat.Abstraction.BaseClass;

namespace UniSpy.Server.Chat.Contract.Result.Channel
{
    public sealed class PartResult : ResultBase
    {
        public string LeaverIRCPrefix { get; set; }
        public bool IsChannelCreator { get; set; }
        public string ChannelName { get; set; }
        public PartResult() { }
    }
}
