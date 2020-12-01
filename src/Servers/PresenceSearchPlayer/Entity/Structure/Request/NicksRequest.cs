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

        public override object Parse()
        {
            var flag = (GPErrorCode)base.Parse();


            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            string md5Password;
            if (!PasswordEncoder.ProcessPassword(_recv, out md5Password))
            {
                return GPErrorCode.NewUserBadPasswords;
            }
            Password = md5Password;


            if (!_recv.ContainsKey("email"))
            {
                return GPErrorCode.Parse;
            }

            RequireUniqueNicks = true;

            if (_recv.ContainsKey("pass"))
            {
                // Old games might send an error is unique nicknames are sent (like GSA 1.0)
                RequireUniqueNicks = false;
            }

            Email = _recv["email"];

            if (_recv.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(_recv["namespaceid"], out namespaceID))
                {
                    return GPErrorCode.Parse;
                }
                NamespaceID = namespaceID;
            }

            return GPErrorCode.NoError;

        }
    }
}
