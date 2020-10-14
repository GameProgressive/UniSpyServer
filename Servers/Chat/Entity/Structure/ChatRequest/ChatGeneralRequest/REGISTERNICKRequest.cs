namespace Chat.Entity.Structure.ChatCommand
{
    public class REGISTERNICKRequest : ChatRequestBase
    {
        public REGISTERNICKRequest(string rawRequest) : base(rawRequest)
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
