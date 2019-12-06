using Chat.Handler.Command.NICK;
using Chat.Handler.Command.USER;
using Chat.Handler.Command.CRYPT;
using Chat.Handler.Command.USRIP;
using Chat.Handler.Command.LOGIN;

namespace Chat.Handler
{
    public class CommandSwitcher
    {
        public static void Switch(ChatSession session, string[] recv)
        {
            string command = recv[0];
            switch (command)
            {
                case "USER":
                    USERHandler.Handle(session, recv);
                    break;
                case "NICK":
                    NICKHandler.Handle(session, recv);
                    break;
                case "CRYPT":
                    CRYPTHandler.Handle(session, recv);
                    break;
                case "USRIP":
                    USRIPHandler.Handle(session, recv);
                    break;
                case "QUIT":
                    session.Disconnect();
                    break;
                case "LOGIN":
                    LOGINHandler.Handle(session, recv);
                    break;
                default:
                    string singleRecv = "";
                    foreach (string data in recv)
                        singleRecv += data;

                    session.ToLog("Unknown request: " + singleRecv);
                    break;
            }

        }
    }
}
