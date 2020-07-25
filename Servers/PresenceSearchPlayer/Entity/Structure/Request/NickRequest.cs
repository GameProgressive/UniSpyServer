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

        public override GPErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!_recv.ContainsKey("email"))
            {
                return GPErrorCode.Parse;
            }

            //First, we try to receive an encoded password
            if (!_recv.ContainsKey("passenc"))
            {
                Password = _recv["pass"];
                //If the encoded password is not sended, we try receiving the password in plain text
                if (!_recv.ContainsKey("pass"))
                {
                    //No password is specified, we cannot continue                   
                    return GPErrorCode.Parse;
                }
            }
            PassEnc = _recv["passenc"];
            Email = _recv["email"];

            return GPErrorCode.NoError;

        }
    }
}
