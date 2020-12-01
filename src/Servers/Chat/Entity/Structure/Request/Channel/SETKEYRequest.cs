using Chat.Abstraction.BaseClass;
using UniSpyLib.Extensions;
using System.Collections.Generic;

namespace Chat.Entity.Structure.ChatCommand
{
    public class SETKEYRequest : ChatRequestBase
    {
        public Dictionary<string, string> KeyValues { get; protected set; }

        public SETKEYRequest(string rawRequest) : base(rawRequest)
        {
            KeyValues = new Dictionary<string, string>();
        }

        public override object Parse()
        {
            if(!(bool)base.Parse())
            {
                return false;
            }

            if (_longParam == null)
                return false;
            KeyValues = StringExtensions.ConvertKVStrToDic(_longParam);
            return true;
        }
    }
}
