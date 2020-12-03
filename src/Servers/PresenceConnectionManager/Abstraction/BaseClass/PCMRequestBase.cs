using UniSpyLib.Extensions;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Abstraction.BaseClass
{
    public abstract class PCMRequestBase : RequestBase
    {
        public new string CommandName { get; protected set; }
        //public uint NamespaceID { get; protected set; }
        public uint OperationID { get; protected set; }

        public new string RawRequest { get; protected set; }
        public Dictionary<string,string> KeyValues{get; protected set;}
        protected Dictionary<string, string> _recv;

        public PCMRequestBase(string rawRequest):base(rawRequest)
        {
            RawRequest = rawRequest;
        }

        public override object Parse()
        {
            KeyValues = UniSpyLib.MiscMethod.GameSpyUtils.ConvertToKeyValue(RawRequest);
            CommandName = KeyValues.Keys.First();

            if (_recv.ContainsKey("id"))
            {
                uint operationID;
                if (!uint.TryParse(_recv["id"], out operationID))
                {
                    return GPErrorCode.Parse;
                }
                OperationID = operationID;
            }
            return GPErrorCode.NoError;
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
