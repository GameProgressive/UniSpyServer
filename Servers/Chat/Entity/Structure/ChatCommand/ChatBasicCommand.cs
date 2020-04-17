using System;
using Chat.Entity.Structure.Enumerator.Request;

namespace Chat.Entity.Structure.ChatCommand
{
    public class ChatBasicCommand:ChatCommandBase
    {
        /// <summary>
        /// basic command do not have command type
        /// </summary>
        public ChatBasicCommand(ChatRequest request) : base(request)
        { }

        public ChatBasicCommand(string request) : base(request)
        {
        }

        public override string BuildCommandString(params string[] param)
        {
            throw new NotImplementedException();
        }
    }
}
