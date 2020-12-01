using UniSpyLib.MiscMethod;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    public class ValidRequest : PSPRequestBase
    {
        public ValidRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public uint NamespaceID { get; protected set; }
        public string Email { get; private set; }
        public string GameName { get; protected set; }

        public override GPErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!_rawRequest.ContainsKey("email") && !GameSpyUtils.IsEmailFormatCorrect(_rawRequest["email"]))
            {
                return GPErrorCode.Parse;
            }

            Email = _rawRequest["email"];

            if (_rawRequest.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(_rawRequest["namespaceid"], out namespaceID))
                {
                    return GPErrorCode.Parse;
                }

                NamespaceID = namespaceID;
            }
            return GPErrorCode.NoError;
        }
    }
}
