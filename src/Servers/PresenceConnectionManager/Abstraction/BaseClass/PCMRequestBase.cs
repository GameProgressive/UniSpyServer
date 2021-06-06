using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Extensions;
using UniSpyLib.MiscMethod;

namespace PresenceConnectionManager.Abstraction.BaseClass
{
    internal abstract class PCMRequestBase : UniSpyRequestBase
    {
        public new string CommandName
        {
            get => (string)base.CommandName;
            protected set => base.CommandName = value;
        }
        public uint OperationID { get; protected set; }

        public new string RawRequest
        {
            get => (string)base.RawRequest;
            set => base.RawRequest = value;
        }
        public Dictionary<string, string> KeyValues { get; protected set; }
        public new GPErrorCode ErrorCode
        {
            get => (GPErrorCode)base.ErrorCode;
            protected set => base.ErrorCode = value;
        }

        public PCMRequestBase()
        {
            ErrorCode = GPErrorCode.NoError;
        }

        public PCMRequestBase(string rawRequest) : base(rawRequest)
        {
            RawRequest = rawRequest;
            ErrorCode = GPErrorCode.NoError;
        }

        public override void Parse()
        {
            KeyValues = GameSpyUtils.ConvertToKeyValue(RawRequest);
            CommandName = KeyValues.Keys.First();

            if (KeyValues.ContainsKey("id"))
            {
                uint operationID;
                if (!uint.TryParse(KeyValues["id"], out operationID))
                {
                    ErrorCode = GPErrorCode.Parse;
                    return;
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
