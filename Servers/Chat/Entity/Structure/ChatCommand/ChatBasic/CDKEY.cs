namespace Chat.Entity.Structure.ChatCommand.ChatBasic
{
    public class CDKEY : ChatCommandBase
    {
        public string CDKey { get; protected set; }

        public CDKEY(string request) : base(request)
        {
        }

        public CDKEY()
        {
        }

        public override bool Parse()
        {
            if (!base.Parse())
            {
                return false;
            }
            CDKey = _cmdParams[0];
            return true;
        }
    }
}
