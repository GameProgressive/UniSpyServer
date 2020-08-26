using System.Collections.Generic;
using GameSpyLib.MiscMethod;
using PresenceSearchPlayer.Entity.Enumerator;
using PresenceSearchPlayer.Entity.Structure.Model;
using PresenceSearchPlayer.Enumerator;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    public class CheckRequest : PSPRequestBase
    {
        // \check\\nick\<nick>\email\<email>\partnerid\0\passenc\<passenc>\gamename\gmtest\final\

        public CheckRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public string Nick { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }

        public override GPError  Parse()
        {
            var flag = base.Parse();
            if (flag != GPError.NoError)
                return flag;
            PasswordEncoder.ProcessPassword(_recv);
            if (!_recv.ContainsKey("nick") || !_recv.ContainsKey("email") || !_recv.ContainsKey("passenc"))
            {
                return GPError.Parse;
            }


            if (!GameSpyUtils.IsEmailFormatCorrect(_recv["email"]))
            {
                return GPError.CheckBadMail;
            }

            Nick = _recv["nick"];
            Password = _recv["passenc"];
            Email = _recv["email"];

            return GPError.NoError;
        }
    }
}
