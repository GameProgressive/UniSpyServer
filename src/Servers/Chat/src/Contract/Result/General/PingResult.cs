using UniSpy.Server.Chat.Abstraction.BaseClass;

namespace UniSpy.Server.Chat.Contract.Result.General
{
    public sealed class PingResult : ResultBase
    {
        public string RequesterIRCPrefix { get; set; }
        public PingResult()
        { }
    }
}
