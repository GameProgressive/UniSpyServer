using UniSpyLib.MiscMethod;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

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

        public override object Parse()
        {
            var flag = (GPErrorCode)base.Parse();
            if (flag != GPErrorCode.NoError)
                return flag;

            string md5Password;
            if (!PasswordEncoder.ProcessPassword(_recv, out md5Password))
            {
                return GPErrorCode.NewUserBadPasswords;
            }
            Password = md5Password;

            if (!_recv.ContainsKey("nick") || !_recv.ContainsKey("email") || Password == null)
            {
                return GPErrorCode.Parse;
            }


            if (!GameSpyUtils.IsEmailFormatCorrect(_recv["email"]))
            {
                return GPErrorCode.CheckBadMail;
            }

            Nick = _recv["nick"];
            Email = _recv["email"];

            return GPErrorCode.NoError;
        }
    }
}
