namespace Chat.Entity.Structure.ChatCommand
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

        public override bool Parse()
        {
            if (!base.Parse())
            {
                return false;
            }
            UserName = _cmdParams[0];
            Hostname = _cmdParams[1];
            ServerName = _cmdParams[2];
            Name = _longParam;
            return true;
        }
    }
}
