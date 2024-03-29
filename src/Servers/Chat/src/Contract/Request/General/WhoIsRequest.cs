using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.General
{
    
    public sealed class WhoIsRequest : RequestBase
    {
        public WhoIsRequest(string rawRequest) : base(rawRequest){ }

        public string NickName { get; private set; }

        public override void Parse()
        {
            base.Parse();

            if (_cmdParams.Count != 1)
            {
                throw new Chat.Exception("The number of IRC cmd params in WHOIS request is incorrect.");
            }

            NickName = _cmdParams[0];
        }
    }
}
