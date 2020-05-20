using Chat.Entity.Structure.ChatUser;
using Chat.Server;
using GameSpyLib.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Entity.Structure.ChatCommand
{
    public class ChatCommandBase
    {
        public string RawRequest { get; protected set; }
        public string CommandName { get; protected set; }
        protected string Prefix;
        protected List<string> _cmdParams;
        protected string _longParam;

        /// <summary>
        /// create instance for Handler
        /// </summary>
        /// <param name="request"></param>
        public ChatCommandBase()
        {
            CommandName = GetType().Name;
        }

        public virtual bool Parse(string recv)
        {
            RawRequest = recv;
            LogWriter.LogCurrentClass(this);
            // at most 2 colon character
            // we do not sure about all command
            // so i block this code here
            List<string> dataFrag = new List<string>();

            if (recv.Where(r => r.Equals(':')).Count() > 2)
            {
                return false;
            }

            int indexOfColon = recv.IndexOf(':');
            if (indexOfColon == 0 && indexOfColon != -1)
            {
                int prefixIndex = recv.IndexOf(' ');
                Prefix = recv.Substring(indexOfColon, prefixIndex);
                recv = recv.Substring(prefixIndex);
            }

            indexOfColon = recv.IndexOf(':');
            if (indexOfColon != 0 && indexOfColon != -1)
            {
                _longParam = recv.Substring(indexOfColon + 1);
                //reset the request string
                recv = recv.Remove(indexOfColon);
            }

            dataFrag = recv.Trim(' ').Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

            if (CommandName == "ChatCommandBase")
            {
                CommandName = dataFrag[0];
            }
            else if (CommandName != dataFrag[0])
            {
                return false;
            }
            if (dataFrag.Count > 1)
            {
                _cmdParams = dataFrag.Skip(1).ToList();
            }
            return true;
        }

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
