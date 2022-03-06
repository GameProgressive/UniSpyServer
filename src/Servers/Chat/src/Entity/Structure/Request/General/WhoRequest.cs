using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.General
{
    public enum WhoRequestType
    {
        GetChannelUsersInfo,
        GetUserInfo
    }
    [RequestContract("WHO")]
    public sealed class WhoRequest : RequestBase
    {
        public WhoRequest(string rawRequest) : base(rawRequest){ }

        public WhoRequestType RequestType { get; private set; }
        public string ChannelName { get; private set; }
        public string NickName { get; private set; }
        public override void Parse()
        {
            base.Parse();

            if (_cmdParams.Count != 1)
            {
                throw new Exception.ChatException("The number of IRC cmd params in WHO request is incorrect.");
            }

            if (_cmdParams[0].Contains("#"))
            {
                RequestType = WhoRequestType.GetChannelUsersInfo;
                ChannelName = _cmdParams[0];
                return;
            }

            RequestType = WhoRequestType.GetUserInfo;
            NickName = _cmdParams[0];
        }
    }
}
