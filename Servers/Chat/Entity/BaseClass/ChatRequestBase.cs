using Chat.Entity.Structure.ChatUser;
using Chat.Server;
using GameSpyLib.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Entity.Structure.ChatCommand
{
    public class ChatRequestBase
    {
        public string RawRequest { get; protected set; }
        public string CmdName { get; protected set; }
        protected string Prefix;
        protected List<string> _cmdParams;
        protected string _longParam;
        /// <summary>
        /// create instance for Handler
        /// </summary>
        /// <param name="request"></param>
        public ChatRequestBase(string rawRequest)
        {
            RawRequest = rawRequest;
        }

        public virtual bool Parse()
        {
            LogWriter.LogCurrentClass(this);

            // at most 2 colon character
            // we do not sure about all command
            // so i block this code here
            List<string> dataFrag = new List<string>();

            if (RawRequest.Where(r => r.Equals(':')).Count() > 2)
            {
                return false;
            }

            int indexOfColon = RawRequest.IndexOf(':');
            if (indexOfColon == 0 && indexOfColon != -1)
            {
                int prefixIndex = RawRequest.IndexOf(' ');
                Prefix = RawRequest.Substring(indexOfColon, prefixIndex);
                RawRequest = RawRequest.Substring(prefixIndex);
            }

            indexOfColon = RawRequest.IndexOf(':');
            if (indexOfColon != 0 && indexOfColon != -1)
            {
                _longParam = RawRequest.Substring(indexOfColon + 1);
                //reset the request string
                RawRequest = RawRequest.Remove(indexOfColon);
            }

            dataFrag = RawRequest.Trim(' ').Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

            CmdName = dataFrag[0];

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
