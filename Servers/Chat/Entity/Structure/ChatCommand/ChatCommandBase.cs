using System;
using Chat.Entity.Structure.Enumerator.Request;

namespace Chat.Entity.Structure.ChatCommand
{
    public abstract class ChatCommandBase
    {
        public string CommandName { get; protected set; }

        protected string[] _requestFrag;

        /// <summary>
        /// create instance for command manager
        /// </summary>
        /// <param name="request"></param>
        public ChatCommandBase(ChatRequest request)
        {
            CommandName = request.ToString();
        }

        /// <summary>
        /// create instance for Handler
        /// </summary>
        /// <param name="request"></param>
        public ChatCommandBase(string request)
        {
            _requestFrag = request.Trim(' ').Split(' ',StringSplitOptions.RemoveEmptyEntries);
            CommandName = _requestFrag[0];
        }

        //Each command must impelemt this method
        public abstract string BuildCommandString(params string[] param);

        public static string BuildCommandString(object cmdCode, string message)
        {
            return BuildCommandString((int)cmdCode, message);
        }
        public static string BuildCommandString(int cmdCode,string message)
        {
            return $":s {cmdCode} {message}\r\n";
        }

    }
}
