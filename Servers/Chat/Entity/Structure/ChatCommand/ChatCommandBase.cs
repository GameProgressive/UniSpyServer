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

        public static string BuildMessageErrorRPL(string errorMsg)
        {
            return BuildMessageRPL("ERROR", errorMsg);
        }

        public static string BuildNumericErrorRPL(ChatError errorCode, string cmdParam, string message)
        {
            return BuildNumericRPL(ChatServer.ServerDomain, (int)errorCode, cmdParam, message);
        }

        public static string BuildNumericErrorRPL(string prefix, ChatError errorCode, string cmdParam, string message)
        {
            return BuildNumericRPL(prefix, (int)errorCode, cmdParam, message);
        }

        public static string BuildNumericRPL(ChatResponseType cmdCode, string cmdParam, string message)
        {
            return BuildNumericRPL("", (int)cmdCode, cmdParam, message);
        }

        public static string BuildNumericRPL(string prefix, ChatResponseType cmdCode, string cmdParam, string message)
        {
            return BuildNumericRPL(prefix, (int)cmdCode, cmdParam, message);
        }

        public static string BuildMessageRPL(string cmdParam, string message)
        {
            return BuildMessageRPL("", cmdParam, message);
        }
        public static string BuildMessageRPL(string prefix, string cmdParam, string message)
        {
            string buffer = "";

            if (prefix != "")
            {
                buffer = $":{prefix} ";
            }

            buffer += $"{cmdParam}";


            if (message != "")
            {
                buffer += $" :{ message}\r\n";
            }
            else
            {
                buffer += "\r\n";
            }

            return buffer;
        }

        protected static string BuildNumericRPL(string prefix, int cmdCode, string cmdParam, string message)
        {
            string asciiCode = cmdCode.ToString();

            if (asciiCode.Length < 3)
            {
                asciiCode = "00" + asciiCode;
            }
            string buffer = "";

            if (prefix != "")
            {
                buffer = $":{prefix} ";
            }

            buffer += $"{asciiCode} {cmdParam}";

            if (message != "")
            {
                buffer += $" :{message}\r\n";
            }
            else
            {
                buffer += "\r\n";
            }

            return buffer;
        }
    }
}
