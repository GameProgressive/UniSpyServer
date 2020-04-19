using System;
namespace Chat.Entity.Structure.ChatCommand
{
    public class PING : ChatCommandBase
    {

        public override string BuildRPL(params string[] param)
        {
            return BuildMessageRPL("D45406C4", GetType().Name);
        }
    }
}
