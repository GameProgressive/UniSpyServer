using System;
using Chat.Entity.Structure.Enumerator.Request;

namespace Chat.Entity.Structure.ChatCommand.ChatMessage
{
    public class PRIVMSG : ChatMessageCommandBase
    {
        public override string BuildCommandString(params string[] param)
        {
            throw new NotImplementedException();
        }
    }
}
