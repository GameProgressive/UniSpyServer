using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class QUITRequest : ChatRequestBase
    {
        public QUITRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string Reason { get; protected set; }

        public override void Parse()
        {
            base.Parse();
            if(!ErrorCode)
            {
               ErrorCode = false;
                return;
            }


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
