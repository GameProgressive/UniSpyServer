using System;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Entity.Structure.ChatCommand
{
    public abstract class ChatCommandBase
    {

        public string CommandName { get; protected set; }
        protected string Prefix;
        protected List<string> _cmdParameters;
        protected string LongParameter;
        private List<string> _requestFrag;

        /// <summary>
        /// create instance for command manager
        /// </summary>
        /// <param name="request"></param>
        public ChatCommandBase()
        {
            CommandName = GetType().Name;
        }

        private string _request;
        /// <summary>
        /// create instance for Handler
        /// </summary>
        /// <param name="request"></param>
        public ChatCommandBase(string request)
        {
            _request = request;
        }

        public virtual bool Parse()
        {
            // at most 2 colon character
            // we do not sure about all command
            // so i block this code here

            if (_request.Where(r => r.Equals(':')).Count() > 2)
            {
                return false;
            }

            int indexOfColon = _request.IndexOf(':');
            if (indexOfColon == 0)
            {
                int prefixIndex = _request.IndexOf(' ');
                Prefix = _request.Substring(indexOfColon, prefixIndex);
                _request = _request.Substring(prefixIndex);
            }

            indexOfColon = _request.IndexOf(':');
            if (indexOfColon != 0)
            {
                LongParameter = _request.Substring(indexOfColon);
                //reset the request string
                _request = _request.Remove(indexOfColon);
            }

            _requestFrag = _request.Trim(' ').Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

            CommandName = _requestFrag[0];
            _cmdParameters = _requestFrag.Skip(1).ToList();
            return true;
        }

        //Some command need impelemt this method
        public virtual string BuildCommandString(params string[] param)
        {
            throw new NotImplementedException();
        }

        public static string BuildCommandString(object cmdCode, string message)
        {
            return BuildCommandString((int)cmdCode, message);
        }
        public static string BuildCommandString(int cmdCode, string message)
        {
            return $":s {cmdCode} {message}\r\n";
        }

    }
}
