using System;
using System.Collections.Generic;
using PresenceSearchPlayer.Entity.Enumerator;
using PresenceSearchPlayer.Entity.Structure.Model;
using PresenceSearchPlayer.Enumerator;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    public class NickRequest : PSPRequestBase
    {
        public NickRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public string Password { get; private set; }
        public string PassEnc { get; private set; }
        public string Email { get; private set; }
        public uint NamespaceID { get; protected set; }
        public override GPError Parse()
        {
            var flag = base.Parse();
            if (flag != GPError.NoError)
            {
                return flag;
            }

            if (!_recv.ContainsKey("email"))
            {
                return GPError.Parse;
            }

            //First, we try to receive an encoded password
            if (!_recv.ContainsKey("passenc"))
            {
                Password = _recv["pass"];
                //If the encoded password is not sended, we try receiving the password in plain text
                if (!_recv.ContainsKey("pass"))
                {
                    //No password is specified, we cannot continue                   
                    return GPError.Parse;
                }
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
