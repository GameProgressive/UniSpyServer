using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.General
{
    
    public sealed class UserRequest : RequestBase
    {
        public UserRequest(string rawRequest) : base(rawRequest){ }

        public string UserName { get; private set; }
        public string Hostname { get; private set; }
        public string ServerName { get; private set; }
        public string NickName { get; private set; }
        public string Name { get; private set; }

        public override void Parse()
        {
            base.Parse();

            UserName = _cmdParams[0];
            Hostname = _cmdParams[1];
            ServerName = _cmdParams[2];
            Name = _longParam;
        }
    }
}