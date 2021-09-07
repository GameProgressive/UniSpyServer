using Chat.Entity.Exception;

namespace Chat.Abstraction.BaseClass
{
    internal class ChannelRequestBase : RequestBase
    {
        public string ChannelName { get; set; }
        public ChannelRequestBase() { }
        public ChannelRequestBase(string rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            base.Parse();

            if (_cmdParams.Count < 1)
            {
                throw new ChatException("channel name is missing.");
            }
            ChannelName = _cmdParams[0];
        }
    }
}
