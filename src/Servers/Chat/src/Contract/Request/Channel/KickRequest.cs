using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.Channel
{
    
    public sealed class KickRequest : ChannelRequestBase
    {
        public new string ChannelName{ get => base.ChannelName; set => base.ChannelName = value; }
        public KickRequest() { }
        public KickRequest(string rawRequest) : base(rawRequest){ }
        public string KickeeNickName { get; set; }
        public string Reason { get; set; }

        public override void Parse()
        {
            base.Parse();

            if (_cmdParams.Count != 2)
            {
                throw new Chat.Exception("The number of IRC parameters are incorrect.");
            }

            KickeeNickName = _cmdParams[1];

            if (_longParam is null)
            {
                throw new Chat.Exception("The IRC long parameters is missing.");
            }

            Reason = _longParam;
        }
    }
}
