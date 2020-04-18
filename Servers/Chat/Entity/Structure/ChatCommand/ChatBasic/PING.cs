using System;
namespace Chat.Entity.Structure.ChatCommand.ChatBasic
{
    public class PING : ChatCommandBase
    {
        public override string BuildCommandString(params string[] param)
        {
            return $"{GetType().Name} :Hello from RetroSpy";
        }
    }
}
