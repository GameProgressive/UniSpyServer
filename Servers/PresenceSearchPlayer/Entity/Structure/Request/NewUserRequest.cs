using System;
using System.Collections.Generic;
using GameSpyLib.MiscMethod;
using PresenceSearchPlayer.Entity.Structure.Model;
using PresenceSearchPlayer.Enumerator;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    public class NewUserRequest : PSPRequestModelBase
    {
        public NewUserRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GPErrorCode Parse()
        {
           var flag = base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!_recv.ContainsKey("nick"))
            {
               return GPErrorCode.Parse;
            }

            if (!_recv.ContainsKey("email") || !GameSpyUtils.IsEmailFormatCorrect(_recv["email"]))
            {
                return GPErrorCode.Parse;
            }

            if (!_recv.ContainsKey("passenc"))
            {
               return GPErrorCode.Parse;
            }
            Nick = _recv["nick"];
            Email = _recv["email"];
            PassEnc = _recv["passenc"];

            if (_recv.ContainsKey("uniquenick"))
            {
                Uniquenick = _recv["uniquenick"];
            }

            return GPErrorCode.NoError;
        }
    }
}
