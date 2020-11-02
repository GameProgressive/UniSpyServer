using UniSpyLib.MiscMethod;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    public class NicksRequest : PSPRequestBase
    {
        public NicksRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public string Password { get; private set; }
        public string Email { get; private set; }
        public uint NamespaceID { get; protected set; }
        public bool RequireUniqueNicks { get; protected set; }

        public override GPError Parse()
        {
            var flag = base.Parse();


            if (flag != GPError.NoError)
            {
                return flag;
            }

            string md5Password;
            if (!PasswordEncoder.ProcessPassword(_rawRequest, out md5Password))
            {
                return GPError.NewUserBadPasswords;
            }
            Password = md5Password;


            if (!_rawRequest.ContainsKey("email"))
            {
                return GPError.Parse;
            }

            RequireUniqueNicks = true;

            if (_rawRequest.ContainsKey("pass"))
            {
                // Old games might send an error is unique nicknames are sent (like GSA 1.0)
                RequireUniqueNicks = false;
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
