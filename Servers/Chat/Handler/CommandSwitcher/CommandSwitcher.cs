using Chat.Handler.CommandHandler.CRYPT;
using Chat.Handler.CommandHandler.LOGIN;
using Chat.Handler.CommandHandler.USRIP;
using Chat.Server;

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
                    new CRYPTHandler(session, cmd);
                    break;
                case "USRIP":
                    new USRIPHandler(session, cmd);
                    break;
                case "LOGIN":
                    new LOGINHandler(session, cmd);
                    break;
                default:
                    if (session.ChatClientProxy == null)
                    {
                        session.ChatClientProxy = new ChatProxy(session);
                        session.ChatClientProxy.Connect();
                    }
                    session.ChatClientProxy.Send(data);
                    break;
            }
        }
    }
}
