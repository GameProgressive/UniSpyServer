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

        protected override bool DetailParse()
        {
           

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
