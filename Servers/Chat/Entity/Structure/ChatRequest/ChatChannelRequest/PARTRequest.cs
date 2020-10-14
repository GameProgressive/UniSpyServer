namespace Chat.Entity.Structure.ChatCommand
{
    public class PARTRequest : ChatChannelRequestBase
    {
        public PARTRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string Reason { get; protected set; }


        protected override bool DetailParse()
        {
            Reason = _longParam;
            return true;
        }
    }
}
