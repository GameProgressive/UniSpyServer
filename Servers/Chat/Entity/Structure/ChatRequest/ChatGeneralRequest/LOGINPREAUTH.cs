namespace Chat.Entity.Structure.ChatCommand
{
    public class LOGINPREAUTH : ChatRequestBase
    {
        public LOGINPREAUTH(string rawRequest) : base(rawRequest)
        {
        }

        public string AuthToken { get; protected set; }
        public string PartnerChallenge { get; protected set; }

        public override bool Parse()
        {
            if (!Parse())
            {
                return false;
            }
            AuthToken = _cmdParams[0];
            PartnerChallenge = _cmdParams[1];
            return true;
        }
    }
}
