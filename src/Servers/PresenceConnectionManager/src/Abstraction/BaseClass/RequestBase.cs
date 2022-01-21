using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using System.Collections.Generic;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Extensions;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass
{
    public abstract class RequestBase : UniSpyRequestBase
    {
        public new string CommandName{ get => (string)base.CommandName;
            protected set => base.CommandName = value; }
        public int OperationID { get; protected set; }

        public new string RawRequest{ get => (string)base.RawRequest; set => base.RawRequest = value; }
        public Dictionary<string, string> RequestKeyValues { get; protected set; }

        public RequestBase()
        {
        }

        public RequestBase(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            RequestKeyValues = GameSpyUtils.ConvertToKeyValue(RawRequest);
            CommandName = RequestKeyValues.Keys.First();

            if (RequestKeyValues.ContainsKey("id"))
            {
                int operationID;
                if (!int.TryParse(RequestKeyValues["id"], out operationID))
                {
                    throw new GPParseException("namespaceid is invalid.");
                }
                OperationID = operationID;
            }
        }

        public static string NormalizeRequest(string message)
        {
            if (message.Contains("login"))
            {
                message = message.Replace(@"\-", @"\");

                int pos = message.IndexesOf("\\")[1];

                if (message.Substring(pos, 2) != "\\\\")
                {
                    message = message.Insert(pos, "\\");
                }
                return message;
            }
            else
            {
                return message;
            }
        }
    }
}
