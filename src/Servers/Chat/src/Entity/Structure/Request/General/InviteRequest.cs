using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.General
{
    [RequestContract("INVITE")]
    public sealed class InviteRequest : RequestBase
    {
        public InviteRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string ChannelName { get; private set; }
        public string UserName { get; private set; }

        public override void Parse()
        {
            base.Parse();

            if (_cmdParams.Count != 2)
            {
                throw new Exception.ChatException("The number of IRC cmd params in GETKEY request is incorrect.");
            }

            ChannelName = _cmdParams[0];
            UserName = _cmdParams[1];
        }
    }
}
