using Chat.Entity.Structure.User;
using Chat.Server;

namespace Chat.Abstraction.BaseClass
{
    public class ChatResponseBase
    {
        public static string BuildResponse(string command)
        {
            return BuildResponse(command, "");
        }

        public static string BuildResponse(string command, string cmdParams)
        {
            return BuildResponse(command, cmdParams, "");
        }

        public static string BuildResponse(string command, string cmdParams, string tailing)
        {
            return BuildResponse(ChatServer.ServerDomain, command, cmdParams, tailing);
        }

        public static string BuildResponse(ChatUserInfo userInfo, string command)
        {
            return BuildResponse(userInfo, command, "");
        }

        public static string BuildResponse(ChatUserInfo userInfo, string command, string cmdParams)
        {
            return BuildResponse(userInfo, command, cmdParams, "");
        }

        /// <summary>
        /// Build the message
        /// </summary>
        /// <param name="userInfo">this is prefix used to indicate the message source</param>
        /// <param name="command"></param>
        /// <param name="cmdParams"></param>
        /// <param name="tailing"></param>
        /// <returns></returns>
        public static string BuildResponse(ChatUserInfo userInfo, string command, string cmdParams, string tailing)
        {
            string prefix = $"{userInfo.NickName}!{userInfo.UserName}@{ChatServer.ServerDomain}";

            return BuildResponse(prefix, command, cmdParams, tailing);
        }

        protected static string BuildResponse(string prefix, string command, string cmdParams, string tailing)
        {
            string buffer = "";

            if (prefix != "")
            {
                buffer = $":{prefix} ";
            }

            buffer += $"{command} {cmdParams}";

            if (tailing != "")
            {
                buffer += $" :{tailing}\r\n";
            }
            else
            {
                buffer += "\r\n";
            }

            return buffer;
        }
    }
}
