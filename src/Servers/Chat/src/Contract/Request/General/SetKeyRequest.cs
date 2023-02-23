using System.Collections.Generic;
using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Core.Extension;

namespace UniSpy.Server.Chat.Contract.Request.General
{
    
    public sealed class SetKeyRequest : RequestBase
    {
        public Dictionary<string, string> KeyValues { get; private set; }

        public SetKeyRequest(string rawRequest) : base(rawRequest){ }

        public override void Parse()
        {
            base.Parse();

            if (_longParam is null)
            {
                throw new Exception.ChatException("The keys and values are missing.");
            }
            KeyValues = StringExtensions.ConvertKVStringToDictionary(_longParam);
        }
    }
}
