using System;

namespace Chat.Entity.Structure.ChatCommand
{
    public class PRIVMSG : ChatMessageCommandBase
    {
        public override string BuildRPL(params string[] param)
        {
            throw new NotImplementedException();
        }
    }
}
