using Chat.Abstraction.BaseClass;
using Chat.Entity.Exception;

namespace Chat.Entity.Structure.Request.General
{
    public enum WHOType
    {
        GetChannelUsersInfo,
        GetUserInfo
    }
    internal sealed class WHORequest : ChatRequestBase
    {
        public WHORequest(string rawRequest) : base(rawRequest)
        {
        }

        //TODO becareful there are channel name
        public string ChannelName { get; protected set; }
        public string NickName { get; protected set; }

        public WHOType RequestType { get; protected set; }
        public override void Parse()
        {
            base.Parse();


            if (_cmdParams.Count != 1)
            {
                throw new ChatException("The number of IRC cmd params in GETKEY request is incorrect.");
            }

            if (_cmdParams[0].Contains("#"))
            {
                RequestType = WHOType.GetChannelUsersInfo;
                ChannelName = _cmdParams[0];
            }
            else
            {
                RequestType = WHOType.GetUserInfo;
                NickName = _cmdParams[0];
            }
        }
    }
}
