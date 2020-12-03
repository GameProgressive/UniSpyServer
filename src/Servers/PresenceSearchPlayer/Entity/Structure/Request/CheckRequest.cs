using UniSpyLib.MiscMethod;
using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    public class CheckRequest : PSPRequestBase
    {
        // \check\\nick\<nick>\email\<email>\partnerid\0\passenc\<passenc>\gamename\gmtest\final\

        public CheckRequest(string rawRequest) :base(rawRequest)
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
            if (!PasswordEncoder.ProcessPassword(RequestKeyValues, out md5Password))
            {
                return GPErrorCode.NewUserBadPasswords;
            }
            Password = md5Password;

            if (!RequestKeyValues.ContainsKey("nick") || !RequestKeyValues.ContainsKey("email") || Password == null)
            {
                return GPErrorCode.Parse;
            }


            if (!GameSpyUtils.IsEmailFormatCorrect(RequestKeyValues["email"]))
            {
                return GPErrorCode.CheckBadMail;
            }

            Nick = RequestKeyValues["nick"];
            Email = RequestKeyValues["email"];

            return GPErrorCode.NoError;
        }
    }
}
