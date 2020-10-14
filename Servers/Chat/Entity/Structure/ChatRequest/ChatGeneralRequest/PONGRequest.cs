namespace Chat.Entity.Structure.ChatCommand
{
    public class PONGRequest : ChatRequestBase
    {
        public PONGRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string EchoMessage { get; protected set; }

        protected override bool DetailParse()
        {

            EchoMessage = _longParam;
            return true;
        }
    }
}
