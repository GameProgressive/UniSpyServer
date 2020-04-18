using System;
namespace Chat.Entity.Structure.ChatCommand.ChatChannel
{
    public class TOPIC : ChatChannelCommandBase
    {
        public TOPIC()
        {
        }

        public TOPIC(string request) : base(request)
        {
        }

        public string Topic { get; protected set; }

        public override bool Parse()
        {
            if (!base.Parse())
            {
                return false;
            }
            if (_longParam == null)
            {
                Topic = "";
            }
            else
            {
                Topic = _longParam;
            }
            return true;
        }
    }
}
