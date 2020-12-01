using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class JOINRequest : ChatChannelRequestBase
    {
        public JOINRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string Password { get; protected set; }

        public override object Parse()
        {
            if(!(bool)base.Parse())
            {
                return false;
            }

            if (_cmdParams.Count > 2)
            {
                return false;
            }
            if (_cmdParams.Count == 2)
            {
                Password = _cmdParams[1];
            }
            return true;
        }
    }
}
