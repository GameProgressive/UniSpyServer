namespace Chat.Entity.Structure.ChatCommand
{
    public enum WHOType
    {
        GetChannelUsersInfo,
        GetUserInfo
    }
    public class WHO : ChatCommandBase
    {
        //TODO becareful there are channel name
        public string ChannelName { get; protected set; }
        public string NickName { get; protected set; }

        public WHOType RequestType { get; protected set; }
        public override bool Parse(string recv)
        {
            if (!base.Parse(recv))
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
                NickName = _cmdParams[0];
            }
            else
            {
                RequestType = WHOType.GetUserInfo;
                ChannelName = _cmdParams[0];
            }
            return true;
        }
    }
}
