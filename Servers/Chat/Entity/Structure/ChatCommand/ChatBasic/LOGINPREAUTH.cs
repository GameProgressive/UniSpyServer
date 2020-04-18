using System;
namespace Chat.Entity.Structure.ChatCommand.ChatBasic
{
    public class LOGINPREAUTH : ChatCommandBase
    {
        public string AuthToken { get; protected set; }
        public string PartnerChallenge { get; protected set; }

        public override bool Parse()
        {
         bool flag=   base.Parse();
            AuthToken = _cmdParameters[0];
            PartnerChallenge = _cmdParameters[1];
            return flag;
        }
    }
}
