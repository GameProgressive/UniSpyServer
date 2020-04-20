using Chat.Server;
using GameSpyLib.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Entity.Structure.ChatCommand
{
    public class ChatCommandBase
    {
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

        public virtual bool Parse(string request)
        {
            LogWriter.LogCurrentClass(this);
            // at most 2 colon character
            // we do not sure about all command
            // so i block this code here
            List<string> dataFrag = new List<string>();

            if (request.Where(r => r.Equals(':')).Count() > 2)
            {
                return false;
            }

            int indexOfColon = request.IndexOf(':');
            if (indexOfColon == 0 && indexOfColon != -1)
            {
                int prefixIndex = request.IndexOf(' ');
                Prefix = request.Substring(indexOfColon, prefixIndex);
                request = request.Substring(prefixIndex);
            }

            indexOfColon = request.IndexOf(':');
            if (indexOfColon != 0 && indexOfColon != -1)
            {
                _longParam = request.Substring(indexOfColon + 1);
                //reset the request string
                request = request.Remove(indexOfColon);
            }

            dataFrag = request.Trim(' ').Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

            if (CommandName == "ChatCommandBase")
            {
                CommandName = dataFrag[0];
            }
            else if (CommandName != dataFrag[0])
            {
                return false;
            }

            _cmdParams = dataFrag.Skip(1).ToList();

            return true;
        }

        //Some command need impelemt this method

        public static string BuildRPL(string prefix, ChatResponseType cmdCode, string cmdParam, string message)
        {
            return BuildRPL(prefix, (int)cmdCode, cmdParam, message);
        }
        public static string BuildMessage(string prefix, string cmdParam, string message)
        {
            string buffer = "";

            if (prefix != "")
            {
                buffer = $":{prefix} ";
            }

            buffer += $"{cmdParam} ";


            if (message != "")
            {
                buffer += $":{ message}\r\n";
            }
            else
            {
                buffer += "\r\n";
            }

            return buffer;
        }
        protected static string BuildRPL(string prefix, int cmdCode, string cmdParam, string message)
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

            buffer += $"{asciiCode} {cmdParam} ";

            if (message != "")
            {
                buffer += $":{ message}\r\n";
            }
            else
            {
                buffer += "\r\n";
            }

            return buffer;
        }


    }
}
