namespace Chat.Entity.Structure.ChatCommand
{
    public class REGISTERNICK : ChatCommandBase
    {
        public string NamespaceID { get; protected set; }
        public string UniqueNick { get; protected set; }
        public string CDKey { get; protected set; }
        public override bool Parse(string request)
        {
            if (!base.Parse(request))
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
