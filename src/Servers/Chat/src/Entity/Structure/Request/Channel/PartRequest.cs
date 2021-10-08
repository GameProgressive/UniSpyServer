using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Exception;

namespace Chat.Entity.Structure.Request
{
    [RequestContract("PART")]
    internal sealed class PartRequest : ChannelRequestBase
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
