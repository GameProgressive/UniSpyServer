namespace Chat.Entity.Structure.ChatCommand
{
    public class CDKEYRequest : ChatRequestBase
    {
        public CDKEYRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string CDKey { get; protected set; }
        
        protected override bool DetailParse()
        {
           
            CDKey = _cmdParams[0];
            return true;
        }
    }
}
