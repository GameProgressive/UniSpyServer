using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc.ChannelInfo;

namespace UniSpy.Server.Chat.Contract.Request.Channel
{

    public sealed class JoinRequest : ChannelRequestBase
    {
        public string Password { get; private set; }
        public PeerRoomType? RoomType { get; private set; }
        public JoinRequest(string rawRequest) : base(rawRequest) { }
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

            RoomType = Aggregate.Misc.ChannelInfo.Channel.GetRoomType(ChannelName);
        }
    }
}
