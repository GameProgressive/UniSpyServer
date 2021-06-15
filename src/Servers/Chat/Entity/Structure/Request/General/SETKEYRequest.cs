using Chat.Abstraction.BaseClass;
using Chat.Entity.Exception;
using System.Collections.Generic;
using UniSpyLib.Extensions;

namespace Chat.Entity.Structure.Request
{
    internal sealed class SETKEYRequest : ChatRequestBase
    {
        public Dictionary<string, string> KeyValues { get; private set; }

        public SETKEYRequest(string rawRequest) : base(rawRequest)
        {
            KeyValues = new Dictionary<string, string>();
        }

        public override void Parse()
        {
            base.Parse();

            if (_longParam == null)
            {
                throw new ChatException("The keys and values are missing.");
            }
            KeyValues = StringExtensions.ConvertKVStringToDictionary(_longParam);
        }
    }
}
