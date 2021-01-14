using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class KICKRequest : ChatChannelRequestBase
    {
        public KICKRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string NickName { get; protected set; }
        public string Reason { get; protected set; }

        public override void Parse()
        {
            base.Parse();
            if (!ErrorCode)
            {
                ErrorCode = false;
                return;
            }

            if (_cmdParams.Count != 2)
            {
                ErrorCode = false;
                return;
            }
            NickName = _cmdParams[1];
            if (_longParam == null)
            {
                ErrorCode = false;
                return;
            }
            Reason = _longParam;
            ErrorCode = true;
        }
    }
}
