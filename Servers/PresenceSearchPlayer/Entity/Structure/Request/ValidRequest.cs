using System;
using System.Collections.Generic;
using GameSpyLib.MiscMethod;
using PresenceSearchPlayer.Entity.Enumerator;
using PresenceSearchPlayer.Entity.Structure.Model;
using PresenceSearchPlayer.Enumerator;

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

        public override GPError  Parse()
        {
            var flag = base.Parse();
            if (flag != GPError.NoError)
            {
                return flag;
            }

            if (!_rawRequest.ContainsKey("email") && !GameSpyUtils.IsEmailFormatCorrect(_rawRequest["email"]))
            {
              return GPError.Parse;
            }

            Email = _rawRequest["email"];

            if (_rawRequest.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(_rawRequest["namespaceid"], out namespaceID))
                {
                    return GPError.Parse;
                }

                NamespaceID = namespaceID;
            }
            return GPError.NoError;
        }
    }
}
