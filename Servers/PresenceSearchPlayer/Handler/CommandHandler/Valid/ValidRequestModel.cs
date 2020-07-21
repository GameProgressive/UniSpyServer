using System;
using System.Collections.Generic;
using GameSpyLib.MiscMethod;
using PresenceSearchPlayer.Entity.Structure.Model;
using PresenceSearchPlayer.Enumerator;

namespace PresenceSearchPlayer.Handler.CommandHandler.Valid
{
    public class ValidRequestModel : PSPRequestModelBase
    {
        public ValidRequestModel(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GPErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GPErrorCode.NoError)
            {
                return flag;
            }

            if (!_recv.ContainsKey("email")&& !GameSpyUtils.IsEmailFormatCorrect(_recv["email"]))
            {
              return GPErrorCode.Parse;
            }

            Email = _recv["email"];
            return GPErrorCode.NoError;
        }
    }
}
