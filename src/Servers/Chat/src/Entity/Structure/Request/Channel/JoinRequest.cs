using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc.ChannelInfo;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel
{
    [RequestContract("JOIN")]
    public sealed class JoinRequest : ChannelRequestBase
    {
        public string Password { get; private set; }
        public PeerRoomType? RoomType { get; private set; }
        public JoinRequest(string rawRequest) : base(rawRequest){ }
        public override void Parse()
        {
            base.Parse();

            if (_cmdParams.Count > 2)
            {
                throw new Exception.ChatException("number of IRC parameters are incorrect.");
            }

            if (_cmdParams.Count == 2)
            {
                Password = _cmdParams[1];
            }

            RoomType = Misc.ChannelInfo.Channel.GetRoomType(ChannelName);
        }
    }
}
