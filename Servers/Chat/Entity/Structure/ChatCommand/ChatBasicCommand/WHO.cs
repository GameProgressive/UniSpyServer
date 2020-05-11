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
        public string Name { get; protected set; }
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

            Name = _cmdParams[0];

            if (Name.Contains("#"))
            {
                RequestType = WHOType.GetChannelUsersInfo;
            }
            else
            {
                RequestType = WHOType.GetUserInfo;
            }
            return true;
        }
    }
}
