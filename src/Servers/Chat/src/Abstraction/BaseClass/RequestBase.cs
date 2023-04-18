using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace UniSpy.Server.Chat.Abstraction.BaseClass
{
    public class RequestBase : UniSpy.Server.Core.Abstraction.BaseClass.RequestBase
    {
        /// <summary>
        /// True means there are no errors
        /// False means there are errors
        /// </summary>
        [JsonProperty]
        public new string RawRequest { get => (string)base.RawRequest; set => base.RawRequest = value; }
        [JsonProperty]
        public new string CommandName { get => (string)base.CommandName; protected set => base.CommandName = value; }
        [JsonProperty]
        protected string _prefix;
        [JsonProperty]
        protected List<string> _cmdParams;
        [JsonProperty]
        protected string _longParam;
        public RequestBase() { }
        /// <summary>
        /// create instance for Handler
        /// </summary>
        /// <param name="request"></param>
        public RequestBase(string rawRequest) : base(rawRequest) { }

        public override void Parse()
        {
            // at most 2 colon character
            // we do not sure about all command
            // so i block this code here
            RawRequest = RawRequest.Replace("\r", "").Replace("\n", "");
            List<string> dataFrag = new List<string>();

            if (RawRequest.Where(r => r.Equals(':')).Count() > 2)
            {
                throw new Chat.Exception($"IRC request is invalid {RawRequest}");
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
        }

        public static string GetCommandName(string request)
        {
            // at most 2 colon character
            // we do not sure about all command
            // so i block this code here
            List<string> dataFrag = new List<string>();

            if (request.Where(r => r.Equals(':')).Count() > 2)
            {
                return null;
            }

            int indexOfColon = request.IndexOf(':');

            string rawRequest = request;
            if (indexOfColon == 0 && indexOfColon != -1)
            {
                int prefixIndex = rawRequest.IndexOf(' ');
                var prefix = rawRequest.Substring(indexOfColon, prefixIndex);
                rawRequest = rawRequest.Substring(prefixIndex);
            }

            indexOfColon = rawRequest.IndexOf(':');
            if (indexOfColon != 0 && indexOfColon != -1)
            {
                var longParam = rawRequest.Substring(indexOfColon + 1);
                //reset the request string
                rawRequest = rawRequest.Remove(indexOfColon);
            }

            dataFrag = rawRequest.Trim(' ').Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

            return dataFrag[0];
        }
    }
}
