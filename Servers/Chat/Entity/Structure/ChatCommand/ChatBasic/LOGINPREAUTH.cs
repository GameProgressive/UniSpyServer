namespace Chat.Entity.Structure.ChatCommand
{
    public class LOGINPREAUTH : ChatCommandBase
    {

        public string AuthToken { get; protected set; }
        public string PartnerChallenge { get; protected set; }

        public override bool Parse(string request)
        {
            if (!base.Parse(request))
            {
                return false;
            }
            AuthToken = _cmdParams[0];
            PartnerChallenge = _cmdParams[1];
            return true;
        }
    }
}
