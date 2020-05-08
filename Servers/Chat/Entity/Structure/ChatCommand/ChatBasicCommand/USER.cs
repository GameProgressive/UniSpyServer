namespace Chat.Entity.Structure.ChatCommand
{
    public class USER : ChatCommandBase
    {
        public string UserName { get; protected set; }
        public string IPAddress { get; protected set; }
        public string ServerName { get; protected set; }
        public string NickName { get; protected set; }
        public string Name { get; protected set; }

        public string BuildResponse(params string[] cmdParam)
        {
            return new PING().BuildResponse();
        }

        public override bool Parse(string recv)
        {
            if (!base.Parse(recv))
            {
                return false;
            }

            UserName = _cmdParams[0];
            IPAddress = _cmdParams[1];
            ServerName = _cmdParams[2];
            Name = _longParam;
            return true;
        }
    }
}
