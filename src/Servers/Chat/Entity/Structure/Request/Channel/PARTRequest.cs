using Chat.Abstraction.BaseClass;
using Chat.Entity.Exception;

namespace Chat.Entity.Structure.Request
{
    internal sealed class PARTRequest : ChatChannelRequestBase
    {
        public new string ChannelName
        {
            get => base.ChannelName;
            set => base.ChannelName = value;
        }
        public string Reason { get; set; }
        public PARTRequest() { }
        public PARTRequest(string rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            base.Parse();
            if (_longParam == null)
            {
                throw new ChatException("The reason of living channel is missing.");
            }
            Reason = _longParam;
        }
    }
}
