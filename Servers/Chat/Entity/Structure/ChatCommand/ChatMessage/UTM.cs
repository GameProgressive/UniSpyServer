namespace Chat.Entity.Structure.ChatCommand
{
    public enum UTMCmdType
    {
        UserUTM,
        ChannelUTM
    }
    public class UTM : ChatMessageCommandBase
    {
        public string NickName { get; protected set; }

        public override bool Parse(string request)
        {
            if (!base.Parse(request))
            {
                return false;
            }


            return true;
        }
    }
}
