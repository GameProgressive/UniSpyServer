using GameSpyLib.MiscMethod;
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

        public override GPError Parse()
        {
            var flag = base.Parse();
            if (flag != GPError.NoError)
                return flag;

            string md5Password;
            if (!PasswordEncoder.ProcessPassword(_rawRequest, out md5Password))
            {
                return GPError.NewUserBadPasswords;
            }
            Password = md5Password;

            if (!_rawRequest.ContainsKey("nick") || !_rawRequest.ContainsKey("email") || Password == null)
            {
                return GPError.Parse;
            }


            if (!GameSpyUtils.IsEmailFormatCorrect(_rawRequest["email"]))
            {
                return GPError.CheckBadMail;
            }

            Nick = _rawRequest["nick"];
            Email = _rawRequest["email"];

            return GPError.NoError;
        }
    }
}
