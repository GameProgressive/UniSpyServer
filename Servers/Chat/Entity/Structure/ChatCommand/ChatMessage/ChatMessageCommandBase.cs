namespace Chat.Entity.Structure.ChatCommand.ChatMessage
{
    public class ChatMessageCommandBase : ChatCommandBase
    {
        public string ChannelName { get; protected set; }
        public string Message { get; protected set; }

        public override bool Parse()
        {
            if (!base.Parse())
            {
                return false;
            }
            ChannelName = _cmdParams[0];
            Message = _longParam;
            return true;
        }
    }
}
