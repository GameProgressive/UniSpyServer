using System;
namespace Chat.Entity.Structure.ChatCommand.ChatBasic
{
    public class MODE:ChatCommandBase
    {
        public MODE(string request) : base(request)
        {
            throw new NotImplementedException("take care of this class");
        }

        public string NickName { get; protected set; }
        public string Mode { get; protected set; }

        public override bool Parse()
        {
            if (!base.Parse())
            {
                return false;
            }
            NickName = _cmdParams[0];
            Mode = _cmdParams[1];
            return true;
        }
    }
}
