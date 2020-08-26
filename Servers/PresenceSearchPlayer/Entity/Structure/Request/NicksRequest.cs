using System;
using System.Collections.Generic;
using GameSpyLib.MiscMethod;
using PresenceSearchPlayer.Entity.Enumerator;
using PresenceSearchPlayer.Entity.Structure.Model;
using PresenceSearchPlayer.Enumerator;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    public class NicksRequest : PSPRequestBase
    {
        public NicksRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public string PassEnc { get; private set; }
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

            PasswordEncoder.ProcessPassword(_recv);

            if (!_recv.ContainsKey("email"))
            {
                return GPError.Parse;
            }

            RequireUniqueNicks = true;

            if (_recv.ContainsKey("pass"))
            {
                // Old games might send an error is unique nicknames are sent (like GSA 1.0)
                RequireUniqueNicks = false;
            }

            PassEnc = _recv["passenc"];
            Email = _recv["email"];

            if (_recv.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(_recv["namespaceid"], out namespaceID))
                {
                    return GPError.Parse;
                }
                NamespaceID = namespaceID;
            }

                return GPError.NoError;

        }
    }
}
