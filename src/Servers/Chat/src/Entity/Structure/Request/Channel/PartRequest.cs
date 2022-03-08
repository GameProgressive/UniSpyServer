using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel
{
    [RequestContract("PART")]
    public sealed class PartRequest : ChannelRequestBase
    {
        public new string ChannelName{ get => base.ChannelName; set => base.ChannelName = value; }
        public string Reason { get; set; }
        public PartRequest() { }
        public PartRequest(string rawRequest) : base(rawRequest){ }
        public override void Parse()
        {
            base.Parse();
            if (_longParam == null)
            {
                throw new Exception.ChatException("The reason of living channel is missing.");
            }
            Reason = _longParam;
        }
    }
}
