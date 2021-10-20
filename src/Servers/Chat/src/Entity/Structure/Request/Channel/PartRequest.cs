using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Exception;

namespace UniSpyServer.Chat.Entity.Structure.Request
{
    [RequestContract("PART")]
    public sealed class PartRequest : ChannelRequestBase
    {
        public new string ChannelName
        {
            get => base.ChannelName;
            set => base.ChannelName = value;
        }
        public string Reason { get; set; }
        public PartRequest() { }
        public PartRequest(string rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            base.Parse();
            if (_longParam == null)
            {
                throw new Exception.Exception("The reason of living channel is missing.");
            }
            Reason = _longParam;
        }
    }
}
