using Chat.Server;

namespace Chat.Entity.Structure.ChatCommand
{
    public class PART : ChatChannelCommandBase
    {
        public string Reason { get; protected set; }


        public override bool Parse(string request)
        {
            if (!base.Parse(request))
            {
                return false;
            }

            Reason = _longParam;
            return true;
        }
    }
}
