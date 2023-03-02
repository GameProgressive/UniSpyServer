using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.General
{
    
    public sealed class InviteRequest : RequestBase
    {
        public string ChannelName { get; private set; }
        public string UserName { get; private set; }
        public InviteRequest(string rawRequest) : base(rawRequest) { }
        public override void Parse()
        {
            base.Parse();

            if (_cmdParams.Count != 2)
            {
                throw new Exception.ChatException("The number of IRC cmd params in GETKEY request is incorrect.");
            }

            ChannelName = _cmdParams[0];
            UserName = _cmdParams[1];
        }
    }
}