using System;
namespace Chat.Entity.Structure.ChatCommand.ChatBasic
{
    public class NAMES:ChatCommandBase
    {
        public NAMES(string request) : base(request)
        {
        }

        public string ChannelName { get; protected set; }

        public override bool Parse()
        {
            if(!base.Parse())
            {
                return false;
            }
            if (_cmdParams.Count != 1)
                return false;   
            ChannelName = _cmdParams[0];
            return true;
        }
    }
}
