using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.Channel
{
    
    public sealed class PartRequest : ChannelRequestBase
    {
        public new string ChannelName{ get => base.ChannelName; set => base.ChannelName = value; }
        public string Reason { get; set; }
        public PartRequest() { }
        public PartRequest(string rawRequest) : base(rawRequest){ }
        public override void Parse()
        {
            base.Parse();
            if (_longParam is null)
            {
                throw new Chat.Exception("The reason of living channel is missing.");
            }
            Reason = _longParam;
        }
    }
}
