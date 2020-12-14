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

        public override void Parse()
        {
            base.Parse();
            if (ErrorCode != GPErrorCode.NoError)
                return;

            string md5Password;
            if (!PasswordEncoder.ProcessPassword(RequestKeyValues, out md5Password))
            {
                ErrorCode = GPErrorCode.NewUserBadPasswords;
            }
            Password = md5Password;

            if (!RequestKeyValues.ContainsKey("nick") || !RequestKeyValues.ContainsKey("email") || Password == null)
            {
                ErrorCode = GPErrorCode.Parse;
                return;
            }


            if (!GameSpyUtils.IsEmailFormatCorrect(RequestKeyValues["email"]))
            {
                ErrorCode = GPErrorCode.CheckBadMail;
                return;
            }

            Nick = RequestKeyValues["nick"];
            Email = RequestKeyValues["email"];

            ErrorCode = GPErrorCode.NoError;
        }
    }
}
