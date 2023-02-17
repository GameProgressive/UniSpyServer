using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.General
{
    
    public sealed class NickRequest : RequestBase
    {
        public NickRequest(string rawRequest) : base(rawRequest){ }

        public string NickName { get; private set; }

        public override void Parse()
        {
            base.Parse();

            if (_cmdParams is not null)
            {
                NickName = _cmdParams[0];
                return;
            }

            if (_longParam is not null)
            {
                NickName = _longParam;
                return;
            }
            
            throw new Exception.ChatException("NICK request is invalid.");
        }
    }
}
