using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Exception.General;
using UniSpyLib.MiscMethod;

namespace PresenceSearchPlayer.Entity.Structure.Request
{
    internal class CheckRequest : PSPRequestBase
    {
        // \check\\nick\<nick>\email\<email>\partnerid\0\passenc\<passenc>\gamename\gmtest\final\
        public CheckRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string Nick { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }

        public override void Parse()
        {
            base.Parse();

            string md5Password;
            if (!PasswordEncoder.ProcessPassword(RequestKeyValues, out md5Password))
            {
                throw new GPParseException("password provided is invalid.");
            }
            Password = md5Password;

            if (!RequestKeyValues.ContainsKey("nick") || !RequestKeyValues.ContainsKey("email") || Password == null)
            {
                throw new GPParseException("check request is incompelete.");
            }

            if (!GameSpyUtils.IsEmailFormatCorrect(RequestKeyValues["email"]))
            {
                throw new GPParseException("email format is incorrect");
            }

            Nick = RequestKeyValues["nick"];
            Email = RequestKeyValues["email"];
        }
    }
}
