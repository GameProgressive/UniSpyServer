using GameStatus.Entity.Enumerate;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.MiscMethod;

namespace GameStatus.Abstraction.BaseClass
{
    public class GSRequestBase : UniSpyRequestBase
    {
        public uint OperationID { get; protected set; }
        public new string CommandName
        {
            get { return (string)base.CommandName; }
            set { base.CommandName = value; }
        }
        public new string RawRequest
        {
            get { return (string)base.RawRequest; }
            set { base.RawRequest = value; }
        }
        public Dictionary<string, string> RequestKeyValues { get; protected set; }

        public GSRequestBase(string rawRequest) : base(rawRequest)
        {
            RawRequest = rawRequest;
            RequestKeyValues = GameSpyUtils.ConvertToKeyValue(rawRequest);
            CommandName = RequestKeyValues.Keys.First();
        }

        public override object Parse()
        {
            if (!RequestKeyValues.ContainsKey("lid") && !RequestKeyValues.ContainsKey("id"))
            {
                return GSError.Parse;
            }

            if (RequestKeyValues.ContainsKey("lid"))
            {
                uint operationID;
                if (!uint.TryParse(RequestKeyValues["lid"], out operationID))
                {
                    return GSError.Parse;
                }
                OperationID = operationID;
            }

            //worms 3d use id not lid so we added an condition here
            if (RequestKeyValues.ContainsKey("id"))
            {
                uint operationID;
                if (!uint.TryParse(RequestKeyValues["id"], out operationID))
                {
                    return GSError.Parse;
                }
                OperationID = operationID;
            }

            return GSError.NoError;
        }
    }
}
