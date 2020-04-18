using System;
using Chat.Entity.Structure.Enumerator.Request;

namespace Chat.Entity.Structure.ChatCommand.ChatMessage
{
    public class ChatMessageCommandBase : ChatCommandBase
    {
        public string ChannelName { get; protected set; }
        public string Message { get; protected set; }

        public override bool Parse()
        {
            bool flag = base.Parse();
            ChannelName = _cmdParameters[0];
            Message = LongParameter;
            return flag;
        }
    }
}
