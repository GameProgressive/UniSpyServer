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

        public string Email { get; private set; }
        public string GameName { get; protected set; }

        public override GPError  Parse()
        {
            var flag = base.Parse();
            if (flag != GPError.NoError)
            {
                return flag;
            }

            if (!_recv.ContainsKey("email")&& !GameSpyUtils.IsEmailFormatCorrect(_recv["email"]))
            {
              return GPError.Parse;
            }

            Email = _recv["email"];
            return GPError.NoError;
        }
    }
}
