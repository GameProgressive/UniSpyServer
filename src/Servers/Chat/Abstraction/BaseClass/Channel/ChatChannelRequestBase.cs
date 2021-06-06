using Chat.Entity.Exception;
using Chat.Entity.Structure;

namespace Chat.Abstraction.BaseClass
{
    public class ChatChannelRequestBase : ChatRequestBase
    {
        public string ChannelName { get; set; }
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
