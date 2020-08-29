using Chat.Server;

namespace Chat.Entity.Structure.ChatCommand
{
    public class PART : ChatChannelRequestBase
    {
        public PART(string rawRequest) : base(rawRequest)
        {
        }

        public string Reason { get; protected set; }


        public override bool Parse()
        {
            if (!Parse())
            {
                return false;
            }

            Reason = _longParam;
            return true;
        }
    }
}
