namespace Chat.Entity.Structure.ChatCommand
{
    public class WHOIS : ChatRequestBase
    {
        public WHOIS(string rawRequest) : base(rawRequest)
        {
        }

        public string NickName { get; protected set; }

        public override bool Parse()
        {
            if (!Parse())
            { return false; }
            if (_cmdParams.Count != 1)
            {
                return false;
            }

            NickName = _cmdParams[0];
            return true;
        }
    }
}
