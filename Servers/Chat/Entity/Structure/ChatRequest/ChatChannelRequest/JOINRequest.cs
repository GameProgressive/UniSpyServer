using Chat.Server;

namespace Chat.Entity.Structure.ChatCommand
{
    public class JOINRequest : ChatChannelRequestBase
    {
        public JOINRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string Password { get; protected set; }

        protected override bool DetailParse()
        {

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
