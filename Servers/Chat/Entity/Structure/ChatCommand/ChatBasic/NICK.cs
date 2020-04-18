namespace Chat.Entity.Structure.ChatCommand.ChatBasic
{
    public class NICK : ChatCommandBase
    {
        public NICK()
        {
        }

        public NICK(string request) : base(request)
        {
        }

        public string NickName { get; protected set; }
        public override bool Parse()
        {
            if (!base.Parse())
            {
                return false;
            }
            NickName = _longParam;
            return true;
        }
    }
}
