namespace Chat.Entity.Structure.ChatCommand
{
    public class ChatMessageCommandBase : ChatCommandBase
    {
        public string ChannelName { get; protected set; }
        public string Message { get; protected set; }

        public override bool Parse(string recv)
        {
            if (!base.Parse(recv))
            {
                return false;
            }
            ChannelName = _cmdParams[0];
            Message = _longParam;
            return true;
        }
    }
}
