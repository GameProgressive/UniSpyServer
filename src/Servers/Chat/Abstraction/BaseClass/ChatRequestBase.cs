using System;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Abstraction.BaseClass
{
    public class ChatRequestBase : UniSpyRequestBase
    {
        /// <summary>
        /// True means there are no errors
        /// False means there are errors
        /// </summary>
        public new bool ErrorCode
        {
            get { return (bool)base.ErrorCode; }
            protected set { base.ErrorCode = value; }
        }
        public new string RawRequest
        {
            get { return (string)base.RawRequest; }
        }

        public new string CommandName
        {
            get { return (string)base.CommandName; }
            set { base.CommandName = value; }
        }

        protected string _prefix;
        protected List<string> _cmdParams;
        protected string _longParam;
        /// <summary>
        /// create instance for Handler
        /// </summary>
        /// <param name="request"></param>
        public ChatRequestBase(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            // at most 2 colon character
            // we do not sure about all command
            // so i block this code here
            List<string> dataFrag = new List<string>();

            if (RawRequest.Where(r => r.Equals(':')).Count() > 2)
            {
               ErrorCode = false;
            }

            int indexOfColon = RawRequest.IndexOf(':');

            string rawRequest = RawRequest;
            if (indexOfColon == 0 && indexOfColon != -1)
            {
                int prefixIndex = rawRequest.IndexOf(' ');
                _prefix = rawRequest.Substring(indexOfColon, prefixIndex);
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

            CommandName = dataFrag[0];

            if (dataFrag.Count > 1)
            {
                _cmdParams = dataFrag.Skip(1).ToList();
            }
            ErrorCode = true;
        }
    }
}
