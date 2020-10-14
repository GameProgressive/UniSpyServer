namespace Chat.Entity.Structure.ChatCommand
{
    public class PARTRequest : ChatChannelRequestBase
    {
        public PARTRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string Reason { get; protected set; }


        public override bool Parse()
        {
            if (!base.Parse())
            {
                return false;
            }

            Reason = _longParam;
            return true;
        }
    }
}
