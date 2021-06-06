using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.MiscMethod;

namespace PresenceSearchPlayer.Abstraction.BaseClass
{
    public class PSPRequestBase : UniSpyRequestBase
    {
        public Dictionary<string, string> RequestKeyValues { get; protected set; }
        public new string RawRequest
        {
            get => (string)base.RawRequest;
        }
        public new string CommandName
        {
            get => (string)base.CommandName;
            protected set => base.CommandName = value;
        }
        public ushort OperationID { get; protected set; }
        public new GPErrorCode ErrorCode
        {
            get => (GPErrorCode)base.ErrorCode;
            set => base.ErrorCode = value;
        }
        public PSPRequestBase(string rawRequest) : base(rawRequest)
        {
            RequestKeyValues = GameSpyUtils.ConvertToKeyValue(rawRequest);
            ErrorCode = GPErrorCode.NoError;
        }

        public override void Parse()
        {
            CommandName = RequestKeyValues.Keys.First();

            if (RequestKeyValues.ContainsKey("id"))
            {
                ushort operationID;
                if (!ushort.TryParse(RequestKeyValues["id"], out operationID))
                {
                    throw new GPGeneralException("operation id is invalid.", GPErrorCode.Parse);
                }
                OperationID = operationID;
            }
        }
    }
}
