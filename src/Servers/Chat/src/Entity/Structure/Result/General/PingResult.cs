using UniSpyServer.Chat.Abstraction.BaseClass;

namespace UniSpyServer.Chat.Entity.Structure.Result.General
{
    public sealed class PingResult : ResultBase
    {
        public string RequesterIRCPrefix { get; set; }
        public PingResult()
        { }
    }
}
