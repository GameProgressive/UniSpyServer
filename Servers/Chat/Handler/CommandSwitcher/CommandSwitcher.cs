using Chat.Handler.CommandHandler.CRYPT;

namespace Chat.Handler.CommandSwitcher
{
    public class CommandSwitcher
    {
        public static void Switch(ChatSession session, string data)
        {
            string message = data.Split("\r\n", System.StringSplitOptions.RemoveEmptyEntries)[0];

            string[] cmd = message.Trim(' ').Split(' ');

            switch (cmd[0])
            {
                case "CRYPT":
                    CRYPTHandler.Handle(session, cmd);
                    break;
                default:
                    session.ChatClientProxy.Send(data);
                    break;
            }

        }
    }
}
