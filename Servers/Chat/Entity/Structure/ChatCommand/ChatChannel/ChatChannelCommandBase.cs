using System;
namespace Chat.Entity.Structure.ChatCommand.ChatChannel
{
    public class ChatChannelCommandBase : ChatCommandBase
    {
        public string ChannelName { get; protected set; }
        public ChatChannelCommandBase()
        {
        }

        public ChatChannelCommandBase(string request) : base(request)
        {
        }
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
