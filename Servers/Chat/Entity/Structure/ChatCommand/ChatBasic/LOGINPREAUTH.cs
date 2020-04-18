using System;
namespace Chat.Entity.Structure.ChatCommand.ChatBasic
{
    public class LOGINPREAUTH : ChatCommandBase
    {
        public LOGINPREAUTH()
        {
        }

        public LOGINPREAUTH(string request) : base(request)
        {
        }

        public string AuthToken { get; protected set; }
        public string PartnerChallenge { get; protected set; }

        public override bool Parse()
        {
            if (!base.Parse())
            {
                return false;
            }
            AuthToken = _cmdParams[0];
            PartnerChallenge = _cmdParams[1];
            return true;
        }
    }
}
