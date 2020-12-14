namespace Chat.Abstraction.BaseClass
{
    public class ChatChannelRequestBase : ChatRequestBase
    {
        public ChatChannelRequestBase(string rawRequest) : base(rawRequest)
        {
        }

        public string ChannelName { get; protected set; }

        public override void Parse()
        {
            base.Parse();
            if (!ErrorCode)
            {
                ErrorCode = false;
            }

            if (_cmdParams.Count < 1)
            {
                ErrorCode = false;
            }
            ChannelName = _cmdParams[0];
            ErrorCode = true;
        }
    }
}
