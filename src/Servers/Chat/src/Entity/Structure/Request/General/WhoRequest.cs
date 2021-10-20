using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Exception;

namespace UniSpyServer.Chat.Entity.Structure.Request.General
{
    public enum WhoRequestType
    {
        GetChannelUsersInfo,
        GetUserInfo
    }
    [RequestContract("WHO")]
    public sealed class WhoRequest : RequestBase
    {
        public WhoRequest(string rawRequest) : base(rawRequest)
        {
        }

        //TODO becareful there are channel name
        public string ChannelName { get; private set; }
        public string NickName { get; private set; }

        public WhoRequestType RequestType { get; private set; }
        public override void Parse()
        {
            base.Parse();


            if (_cmdParams.Count != 1)
            {
                throw new Exception.Exception("The number of IRC cmd params in GETKEY request is incorrect.");
            }

            if (_cmdParams[0].Contains("#"))
            {
                RequestType = WhoRequestType.GetChannelUsersInfo;
                ChannelName = _cmdParams[0];
            }
            else
            {
                RequestType = WhoRequestType.GetUserInfo;
                NickName = _cmdParams[0];
            }
        }
    }
}
