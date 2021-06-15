using Chat.Entity.Exception;

namespace Chat.Abstraction.BaseClass
{
    internal class ChatChannelRequestBase : ChatRequestBase
    {
        public string ChannelName { get; protected set; }
        public ChatChannelRequestBase() { }
        public ChatChannelRequestBase(string rawRequest) : base(rawRequest)
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
