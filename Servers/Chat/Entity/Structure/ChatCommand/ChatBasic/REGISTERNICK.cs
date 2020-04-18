namespace Chat.Entity.Structure.ChatCommand.ChatBasic
{
    public class REGISTERNICK : ChatCommandBase
    {
        public REGISTERNICK()
        {
        }

        public REGISTERNICK(string request) : base(request)
        {
        }

        public string NamespaceID { get; protected set; }
        public string UniqueNick { get; protected set; }
        public string CDKey { get; protected set; }
        public override bool Parse()
        {
            if (!base.Parse())
            {
                return false;
            }
            NamespaceID = _cmdParams[0];
            UniqueNick = _cmdParams[1];
            CDKey = _cmdParams[2];
            return true;
        }
    }
}
