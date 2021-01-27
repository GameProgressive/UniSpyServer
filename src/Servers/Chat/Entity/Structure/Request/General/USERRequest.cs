using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request
{
    public class USERRequest : ChatRequestBase
    {
        public USERRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string UserName { get; protected set; }
        public string Hostname { get; protected set; }
        public string ServerName { get; protected set; }
        public string NickName { get; protected set; }
        public string Name { get; protected set; }

        public override void Parse()
        {
            base.Parse();
            if(ErrorCode != ChatErrorCode.NoError)
            {
                ErrorCode = ChatErrorCode.Parse;
                return;
            }
            UserName = _cmdParams[0];
            Hostname = _cmdParams[1];
            ServerName = _cmdParams[2];
            Name = _longParam;
        }
    }
}
