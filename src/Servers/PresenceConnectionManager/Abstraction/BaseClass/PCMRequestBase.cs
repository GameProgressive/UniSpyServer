using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Abstraction.BaseClass
{
    public abstract class PCMRequestBase : IRequest
    {
        public string CommandName { get; protected set; }
        //public uint NamespaceID { get; protected set; }
        public uint OperationID { get; protected set; }

        object IRequest.CommandName => CommandName;

        protected Dictionary<string, string> _recv;

        public PCMRequestBase(Dictionary<string, string> recv)
        {
            _recv = recv;
            CommandName = _recv.Keys.First();
        }

        public virtual GPErrorCode Parse()
        {
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

        object IRequest.Parse()
        {
            return Parse();
        }

        public object GetInstance()
        {
            return this;
        }
    }
}
