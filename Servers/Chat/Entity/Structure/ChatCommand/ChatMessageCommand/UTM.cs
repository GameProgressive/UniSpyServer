namespace Chat.Entity.Structure.ChatCommand
{
    public enum UTMCmdType
    {
        UserUTM,
        ChannelUTM
    }

    public class UTM : ChatMessageCommandBase
    {
        public UTMCmdType RequestType { get; protected set; }
        public string NickName { get; protected set; }

        public override bool Parse(string request)
        {
            if (!base.Parse(request))
            {
                return false;
            }
            if (_cmdParams.Count != 1)
            {
                return false;
            }

            if (_cmdParams[0].Contains("#"))
            {
                RequestType = UTMCmdType.ChannelUTM;
            }
            else
            {
                RequestType = UTMCmdType.UserUTM;
                NickName = _cmdParams[0];
            }

            return true;
        }
    }
}
