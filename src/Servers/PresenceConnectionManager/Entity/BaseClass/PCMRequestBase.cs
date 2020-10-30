using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Extensions;
using PresenceSearchPlayer.Entity.Enumerator;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Entity.BaseClass
{
    public abstract class PCMRequestBase : IRequest
    {
        public string CmdName { get; protected set; }
        //public uint NamespaceID { get; protected set; }
        public uint OperationID { get; protected set; }

        protected Dictionary<string, string> _recv;

        public PCMRequestBase(Dictionary<string, string> recv)
        {
            _recv = recv;
            CmdName = _recv.Keys.First();
        }

        public virtual GPError Parse()
        {
            if (_recv.ContainsKey("id"))
            {
                uint operationID;
                if (!uint.TryParse(_recv["id"], out operationID))
                {
                    return GPError.Parse;
                }
                OperationID = operationID;
            }
            return GPError.NoError;
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
