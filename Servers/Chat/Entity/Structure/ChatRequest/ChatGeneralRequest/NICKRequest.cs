using Chat.Entity.Structure.ChatResponse;
using Chat.Entity.Structure.ChatUser;

namespace Chat.Entity.Structure.ChatCommand
{
    public class NICKRequest : ChatRequestBase
    {
        public NICKRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string NickName { get; protected set; }

        public override bool Parse()
        {
            if (!base.Parse())
            {
                return false;
            }

            if (_cmdParams.Count == 0)
            {
                NickName = _longParam;
            }
            else
            {
                NickName = _cmdParams[0];
            }
            return true;
        }
    }
}
