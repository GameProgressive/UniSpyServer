using Chat.Abstraction.BaseClass;
using Chat.Entity.Exception;

namespace Chat.Entity.Structure.Request.General
{
    internal sealed class NICKRequest : ChatRequestBase
    {
        public NICKRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string NickName { get; private set; }

        public override void Parse()
        {
            base.Parse();

            if (_cmdParams != null)
            {
                NickName = _cmdParams[0];
            }
            else if (_longParam != null)
            {
                NickName = _longParam;
            }
            else
            {
                throw new ChatException("NICK request is invalid.");
            }


        }
    }
}
