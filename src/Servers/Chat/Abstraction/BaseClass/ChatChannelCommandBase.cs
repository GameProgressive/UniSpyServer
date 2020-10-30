namespace Chat.Abstraction.BaseClass
{
    public class ChatChannelRequestBase : ChatRequestBase
    {
        public ChatChannelRequestBase(string rawRequest) : base(rawRequest)
        {
        }

        public string ChannelName { get; protected set; }

        public override bool Parse()
        {
            if (!base.Parse())
            {
                return false;
            }

            if (_cmdParams.Count < 1)
            {
                return false;
            }
            ChannelName = _cmdParams[0];
            return true;
        }
    }
}
