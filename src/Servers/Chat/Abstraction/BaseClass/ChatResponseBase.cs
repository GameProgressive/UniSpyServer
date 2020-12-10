using Chat.Entity.Structure.User;
using Chat.Network;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Abstraction.BaseClass
{
    public class ChatResponseBase:UniSpyResponseBase
    {
        public ChatResponseBase(object result) : base(result)
        {
        }

        public static string BuildRPL(string command)
        {
            return BuildRPL(command, "");
        }

        public static string BuildRPL(string command, string cmdParams)
        {
            return BuildRPL(command, cmdParams, "");
        }

        public static string BuildRPL(string command, string cmdParams, string tailing)
        {
            return BuildRPL(ChatServer.ServerDomain, command, cmdParams, tailing);
        }

        public static string BuildRPL(ChatUserInfo userInfo, string command)
        {
            return BuildRPL(userInfo, command, "");
        }

        public static string BuildRPL(ChatUserInfo userInfo, string command, string cmdParams)
        {
            return BuildRPL(userInfo, command, cmdParams, "");
        }

        /// <summary>
        /// Build the message
        /// </summary>
        /// <param name="userInfo">this is prefix used to indicate the message source</param>
        /// <param name="command"></param>
        /// <param name="cmdParams"></param>
        /// <param name="tailing"></param>
        /// <returns></returns>
        public static string BuildRPL(ChatUserInfo userInfo, string command, string cmdParams, string tailing)
        {
            string prefix = $"{userInfo.NickName}!{userInfo.UserName}@{ChatServer.ServerDomain}";

            return BuildRPL(prefix, command, cmdParams, tailing);
        }

        protected static string BuildRPL(string prefix, string command, string cmdParams, string tailing)
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

        public override object Build()
        {
            return true;
        }
    }
}
