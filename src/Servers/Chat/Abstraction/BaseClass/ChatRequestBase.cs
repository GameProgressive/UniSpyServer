using System;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Abstraction.BaseClass
{
    public class ChatRequestBase : RequestBase
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

        public ChatRequestBase(ChatRequestBase request)
        {
            RawRequest = request.RawRequest;
        }

        public virtual bool Parse()
        {
            // at most 2 colon character
            // we do not sure about all command
            // so i block this code here
            List<string> dataFrag = new List<string>();

            if (RawRequest.Where(r => r.Equals(':')).Count() > 2)
            {
                return false;
            }

            int indexOfColon = RawRequest.IndexOf(':');

            string rawRequest = RawRequest;
            if (indexOfColon == 0 && indexOfColon != -1)
            {
                int prefixIndex = rawRequest.IndexOf(' ');
                Prefix = rawRequest.Substring(indexOfColon, prefixIndex);
                rawRequest = rawRequest.Substring(prefixIndex);
            }

            indexOfColon = rawRequest.IndexOf(':');
            if (indexOfColon != 0 && indexOfColon != -1)
            {
                _longParam = rawRequest.Substring(indexOfColon + 1);
                //reset the request string
                rawRequest = rawRequest.Remove(indexOfColon);
            }

            dataFrag = rawRequest.Trim(' ').Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

            CmdName = dataFrag[0];

            if (dataFrag.Count > 1)
            {
                _cmdParams = dataFrag.Skip(1).ToList();
            }
            return true;
        }
    }
}
