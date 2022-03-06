using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel
{
    [RequestContract("KICK")]
    public sealed class KickRequest : ChannelRequestBase
    {
        public new string ChannelName{ get => base.ChannelName; set => base.ChannelName = value; }
        public KickRequest() { }
        public KickRequest(string rawRequest) : base(rawRequest){ }
        public string KickeeNickName { get; set; }
        public string Reason { get; set; }

        public override void Parse()
        {
            base.Parse();

            if (_cmdParams.Count != 1)
            {
                throw new Exception.ChatException("The number of IRC parameters are incorrect.");
            }

            KickeeNickName = _cmdParams[0];

            if (_longParam == null)
            {
                throw new Exception.ChatException("The IRC long parameters is missing.");
            }

            Reason = _longParam;
        }
    }
}
