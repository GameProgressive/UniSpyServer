using System.Collections.Generic;
using GameSpyLib.Extensions;
using PresenceSearchPlayer.Entity.Enumerator;

namespace PresenceConnectionManager.Entity.BaseClass
{
    public abstract class PCMRequestBase
    {
        protected Dictionary<string, string> _recv;
        //public uint NamespaceID { get; protected set; }
        public uint OperationID { get; protected set; }

        public PCMRequestBase(Dictionary<string,string> recv)
        {
            _recv = recv;
        }

        public virtual GPError  Parse()
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

        public static string RequstFormatConversion(string message)
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
