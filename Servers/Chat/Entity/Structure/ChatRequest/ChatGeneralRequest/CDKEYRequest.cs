namespace Chat.Entity.Structure.ChatCommand
{
    public class CDKEYRequest : ChatRequestBase
    {
        public CDKEYRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string CDKey { get; protected set; }
        
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
