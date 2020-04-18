using System;
namespace Chat.Entity.Structure.ChatCommand.ChatChannel
{
    public class PART : ChatChannelCommandBase
    {
        public string Reason { get; protected set; }
        public PART()
        {
        }

        public PART(string request) : base(request)
        {
        }

        public override bool Parse()
        {
            if (!base.Parse())
            {
                return false;
            }

            Reason = _longParam;
            return true;
        }
    }
}
