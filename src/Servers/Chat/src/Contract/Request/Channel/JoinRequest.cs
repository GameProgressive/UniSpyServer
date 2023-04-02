using UniSpy.Server.Chat.Abstraction.BaseClass;

namespace UniSpy.Server.Chat.Contract.Request.Channel
{

    public sealed class JoinRequest : ChannelRequestBase
    {
        public string Password { get; private set; }
        public JoinRequest(string rawRequest) : base(rawRequest) { }
        public override void Parse()
        {
            base.Parse();

            if (_cmdParams.Count > 2)
            {
                throw new Chat.Exception("number of IRC parameters are incorrect.");
            }

            if (_cmdParams.Count == 2)
            {
                Password = _cmdParams[1];
            }
        }
    }
}
