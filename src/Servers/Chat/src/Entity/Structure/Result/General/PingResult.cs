using UniSpyServer.Servers.Chat.Abstraction.BaseClass;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Result.General
{
    public sealed class PingResult : ResultBase
    {
        public string RequesterIRCPrefix { get; set; }
        public PingResult()
        { }
    }
}
