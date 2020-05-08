namespace Chat.Entity.Structure.ChatCommand
{
    public class ChatChannelCommandBase : ChatCommandBase
    {
        public string ChannelName { get; protected set; }

        public override bool Parse(string recv)
        {
            if (!base.Parse(recv))
            {
                return false;
            }
            if (_cmdParams.Count < 1)
            {
                return false;
            }
            ChannelName = _cmdParams[0];
            return true;
        }
    }
}
