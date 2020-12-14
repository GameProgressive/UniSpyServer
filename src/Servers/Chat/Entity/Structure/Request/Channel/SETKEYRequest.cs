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

        public override void Parse()
        {
            base.Parse();
            if(!ErrorCode)
            {
               ErrorCode = false;
                return;
            }

            if (_longParam == null)
            {
                ErrorCode = false;
            }
            KeyValues = StringExtensions.ConvertKVStrToDic(_longParam);
            ErrorCode = true;
        }
    }
}
