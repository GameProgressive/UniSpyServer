using GameStatus.Entity.Enumerate;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.MiscMethod;

namespace GameStatus.Abstraction.BaseClass
{
    public class GSRequestBase : RequestBase
    {
        public uint OperationID { get; protected set; }
        public new string CommandName { get; protected set; }
        public new string RawRequest { get; protected set; }
        public Dictionary<string, string> KeyValues { get; protected set; }

        public GSRequestBase(string rawRequest) : base(rawRequest)
        {
            RawRequest = rawRequest;
            KeyValues = GameSpyUtils.ConvertToKeyValue(rawRequest);
            CommandName = KeyValues.Keys.First();
        }

        public override object Parse()
        {
            if (!KeyValues.ContainsKey("lid") && !KeyValues.ContainsKey("id"))
            {
                return GSError.Parse;
            }

            if (KeyValues.ContainsKey("lid"))
            {
                uint operationID;
                if (!uint.TryParse(KeyValues["lid"], out operationID))
                {
                    return GSError.Parse;
                }
                OperationID = operationID;
            }

            //worms 3d use id not lid so we added an condition here
            if (KeyValues.ContainsKey("id"))
            {
                uint operationID;
                if (!uint.TryParse(KeyValues["id"], out operationID))
                {
                    return GSError.Parse;
                }
                OperationID = operationID;
            }

            return GSError.NoError;
        }
    }
}
