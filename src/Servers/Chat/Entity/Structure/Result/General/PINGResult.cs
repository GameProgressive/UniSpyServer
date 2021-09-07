using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result.General
{
    internal sealed class PINGResult : ResultBase
    {
        public string RequesterIRCPrefix { get; set; }
        public PINGResult()
        { }
    }
}
