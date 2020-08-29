using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatResponse;

namespace Chat.Entity.Structure.ChatCommand
{
    public class KICKRequest : ChatChannelRequestBase
    {
        public KICKRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string NickName { get; protected set; }
        public string Reason { get; protected set; }

        public override bool Parse()
        {
            if (!base.Parse())
                return false;
            if (_cmdParams.Count != 2)
            {
                return false;
            }
            NickName = _cmdParams[1];
            if (_longParam == null)
                return false;
            Reason = _longParam;
            return true;
        }
    }
}
