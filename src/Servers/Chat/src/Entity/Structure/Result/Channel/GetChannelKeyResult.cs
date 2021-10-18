using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result
{
    public sealed class GetChannelKeyResult : ResultBase
    {
        public string ChannelUserIRCPrefix { get; set; }
        public string ChannelName { get; set; }
        public string Values { get; set; }
        public GetChannelKeyResult()
        {
        }
    }
}
