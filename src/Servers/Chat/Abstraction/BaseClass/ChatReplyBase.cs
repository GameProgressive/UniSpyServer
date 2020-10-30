using Chat.Entity.Structure.ChatUser;
using Chat.Server;

namespace Chat.Abstraction.BaseClass
{
    public class ChatReplyBase
    {
        public static string BuildReply(string command)
        {
            return BuildReply(command, "");
        }

        public static string BuildReply(string command, string cmdParams)
        {
            return BuildReply(command, cmdParams, "");
        }

        public static string BuildReply(string command, string cmdParams, string tailing)
        {
            return BuildReply(ChatServer.ServerDomain, command, cmdParams, tailing);
        }

        public static string BuildReply(ChatUserInfo userInfo, string command)
        {
            return BuildReply(userInfo, command, "");
        }

        public static string BuildReply(ChatUserInfo userInfo, string command, string cmdParams)
        {
            return BuildReply(userInfo, command, cmdParams, "");
        }

        /// <summary>
        /// Build the message
        /// </summary>
        /// <param name="userInfo">this is prefix used to indicate the message source</param>
        /// <param name="command"></param>
        /// <param name="cmdParams"></param>
        /// <param name="tailing"></param>
        /// <returns></returns>
        public static string BuildReply(ChatUserInfo userInfo, string command, string cmdParams, string tailing)
        {
            string prefix = $"{userInfo.NickName}!{userInfo.UserName}@{ChatServer.ServerDomain}";

            return BuildReply(prefix, command, cmdParams, tailing);
        }

        protected static string BuildReply(string prefix, string command, string cmdParams, string tailing)
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
