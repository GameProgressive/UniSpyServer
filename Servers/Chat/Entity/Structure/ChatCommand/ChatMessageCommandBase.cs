namespace Chat.Entity.Structure.ChatCommand
{
    public enum ChatMessageType
    {
        ChannelMessage,
        UserMessage
    }

    public class ChatMessageCommandBase : ChatChannelCommandBase
    {
        public ChatMessageType RequestType { get; protected set; }
        public string NickName { get; protected set; }
        public string Message { get; protected set; }

        public override bool Parse(string recv)
        {
            if (!base.Parse(recv))
            {
                return false;
            }

            if (_cmdParams[0].Contains("#"))
            {
                RequestType = ChatMessageType.ChannelMessage;
            }
            else
            {
                RequestType = ChatMessageType.UserMessage;
                ChannelName = null;
                NickName = _cmdParams[0];
            }

            Message = _longParam;
            return true;
        }
    }
}
