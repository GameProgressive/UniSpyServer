using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result.General
{
    internal sealed class PingResult : ResultBase
    {
        public string RequesterIRCPrefix { get; set; }
        public PingResult()
        { }
    }
}
