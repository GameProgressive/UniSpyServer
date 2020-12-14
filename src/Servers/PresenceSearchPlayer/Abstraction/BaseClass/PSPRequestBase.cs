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
        public new string RawRequest { get; protected set; }
        public new string CommandName { get; protected set; }
        public ushort OperationID { get; protected set; }
        public new GPErrorCode ErrorCode
        {
            get { return (GPErrorCode)base.ErrorCode; }
            protected set { base.ErrorCode = value; }
        }

        public PSPRequestBase(string rawRequest) : base(rawRequest)
        {
            RawRequest = rawRequest;
        }

        public override void Parse()
        {
            RequestKeyValues = GameSpyUtils.ConvertToKeyValue(RawRequest);
            CommandName = RequestKeyValues.Keys.First();

            if (RequestKeyValues.ContainsKey("id"))
            {
                ushort operationID;
                if (!ushort.TryParse(RequestKeyValues["id"], out operationID))
                {
                    ErrorCode = GPErrorCode.Parse;
                    return;
                }
                OperationID = operationID;
            }

            ErrorCode = GPErrorCode.NoError;
        }
    }
}
