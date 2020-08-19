using Chat.Entity.Structure.ChatResponse;

namespace Chat.Entity.Structure.ChatCommand
{
    public class USRIP : ChatCommandBase
    {
        public USRIP()
        {
        }

        public override bool Parse(string recv)
        {
            return true; // USRIP content is empty!
        }
    }
}
