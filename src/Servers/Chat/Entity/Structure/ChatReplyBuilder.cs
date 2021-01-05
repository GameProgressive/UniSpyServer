using System;
using Chat.Entity.Structure.User;
using Chat.Network;

namespace Chat.Entity.Structure
{
    public class ChatReplyBuilder
    {
        public static string Build(string command)
        {
            return Build(command, "");
        }

        public static string Build(string command, string cmdParams)
        {
            return Build(command, cmdParams, "");
        }

        public static string Build(string command, string cmdParams, string tailing)
        {
            return Build(ChatServer.ServerDomain, command, cmdParams, tailing);
        }

        public static string Build(ChatUserInfo userInfo, string command)
        {
            return Build(userInfo, command, "");
        }

        public static string Build(ChatUserInfo userInfo, string command, string cmdParams)
        {
            return Build(userInfo, command, cmdParams, "");
        }

        /// <summary>
        /// Build the message
        /// </summary>
        /// <param name="userInfo">this is prefix used to indicate the message source</param>
        /// <param name="command"></param>
        /// <param name="cmdParams"></param>
        /// <param name="tailing"></param>
        /// <returns></returns>
        public static string Build(ChatUserInfo userInfo, string command, string cmdParams, string tailing)
        {
            string prefix = $"{userInfo.NickName}!{userInfo.UserName}@{ChatServer.ServerDomain}";

            return Build(prefix, command, cmdParams, tailing);
        }

        protected static string Build(string prefix, string command, string cmdParams, string tailing)
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
