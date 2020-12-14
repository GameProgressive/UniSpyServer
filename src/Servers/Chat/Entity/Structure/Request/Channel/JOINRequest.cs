using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class JOINRequest : ChatChannelRequestBase
    {
        public JOINRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string Password { get; protected set; }

        public override void Parse()
        {
            base.Parse();
            if(!ErrorCode)
            {
               ErrorCode = false;
                return;
            }

            if (_cmdParams.Count > 2)
            {
               ErrorCode = false;
                return;
            }

            if (_cmdParams.Count == 2)
            {
                Password = _cmdParams[1];
            }

            ErrorCode = true;
        }
    }
}
