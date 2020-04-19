using System;
namespace Chat.Entity.Structure.ChatCommand
{
    public class MODE : ChatCommandBase
    {
        public string NickName { get; protected set; }
        public string Mode { get; protected set; }

        public override bool Parse(string request)
        {
            if (!base.Parse(request))
            {
                return false;
            }
            NickName = _cmdParams[0];
            Mode = _cmdParams[1];
            return true;
        }
    }
}
