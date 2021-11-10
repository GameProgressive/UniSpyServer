using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel
{
    [RequestContract("JOIN")]
    public sealed class JoinRequest : ChannelRequestBase
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
