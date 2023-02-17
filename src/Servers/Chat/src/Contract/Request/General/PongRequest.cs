using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.General
{
    
    public sealed class PongRequest : RequestBase
    {
        public PongRequest(string rawRequest) : base(rawRequest){ }

        public string EchoMessage { get; private set; }

        public override void Parse()
        {
            base.Parse();

            if (_longParam is null)
            {
                throw new Exception.ChatException("Echo message is missing.");
            }
            EchoMessage = _longParam;
        }
    }
}
