using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Exception;

namespace Chat.Entity.Structure.Request
{
    [RequestContract("JOIN")]
    internal sealed class JoinRequest : ChannelRequestBase
    {
        public JoinRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string Password { get; private set; }

        public override void Parse()
        {
            base.Parse();

            if (_cmdParams.Count > 2)
            {
                throw new Exception.Exception("number of IRC parameters are incorrect.");
            }

            if (_cmdParams.Count == 2)
            {
                Password = _cmdParams[1];
            }
        }
    }
}
