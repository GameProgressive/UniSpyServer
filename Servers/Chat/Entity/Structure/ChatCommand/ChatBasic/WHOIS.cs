using System;
namespace Chat.Entity.Structure.ChatCommand.ChatBasic
{
    public class WHOIS : ChatCommandBase
    {
        public string UserName { get; protected set; }
        public WHOIS(string request) : base(request)
        {
        }
        public override bool Parse()
        {
            if(! base.Parse())
            { return false; }
            if(_cmdParams.Count!=1)
            {
                return false;
            }

            UserName = _cmdParams[0];
            return true;
        }
    }
}
