using System.Collections.Generic;
using UniSpyServer.Servers.Chat.Abstraction.BaseClass;

using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.General
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
