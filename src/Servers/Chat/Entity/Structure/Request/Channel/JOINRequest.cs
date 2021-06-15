using Chat.Abstraction.BaseClass;
using Chat.Entity.Exception;

namespace Chat.Entity.Structure.Request
{
    internal sealed class JOINRequest : ChatChannelRequestBase
    {
        public JOINRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string Password { get; private set; }

        public override void Parse()
        {
            base.Parse();

            if (_cmdParams.Count > 2)
            {
                throw new ChatException("number of IRC parameters are incorrect.");
            }

            if (_cmdParams.Count == 2)
            {
                Password = _cmdParams[1];
            }
        }
    }
}
