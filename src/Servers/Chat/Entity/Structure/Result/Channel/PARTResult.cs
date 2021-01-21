using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result
{
    internal sealed class PARTResult : ChatResultBase
    {
        public string LeaverIRCPrefix { get; set; }
        public string ChannelName { get; set; }
        public string Message { get; set; }
        public PARTResult()
        {
        }
    }
}
