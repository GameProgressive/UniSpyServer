namespace Chat.Entity.Structure.ChatCommand
{
    public enum WHOType
    {
        GetChannelUsersInfo,
        GetUserInfo
    }
    public class WHORequest : ChatRequestBase
    {
        public WHORequest(string rawRequest) : base(rawRequest)
        {
        }

        //TODO becareful there are channel name
        public string ChannelName { get; protected set; }
        public string NickName { get; protected set; }

        public WHOType RequestType { get; protected set; }
        public override bool Parse()
        {
            if (!Parse())
            {
                return false;
            }
            if (_cmdParams.Count != 1)
            {
                return false;
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
            return true;
        }
    }
}
