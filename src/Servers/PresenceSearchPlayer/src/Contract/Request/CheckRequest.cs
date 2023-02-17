using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;
using UniSpy.Server.Core.MiscMethod;

namespace UniSpy.Server.PresenceSearchPlayer.Contract.Request
{
    
    public sealed class CheckRequest : RequestBase
    {
        // \check\\nick\<nick>\email\<email>\partnerid\0\passenc\<passenc>\gamename\gmtest\final\
        public CheckRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string Nick { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public int? PartnerId { get; private set; }
        public override void Parse()
        {
            base.Parse();
            Password = PasswordEncoder.ProcessPassword(RequestKeyValues);
            if (!RequestKeyValues.ContainsKey("nick") || !RequestKeyValues.ContainsKey("email") || Password is null)
            {
                throw new GPParseException("check request is incompelete.");
            }

            if (!GameSpyUtils.IsEmailFormatCorrect(RequestKeyValues["email"]))
            {
                throw new GPParseException("email format is incorrect");
            }

            Nick = RequestKeyValues["nick"];
            Email = RequestKeyValues["email"];

            if (RequestKeyValues.ContainsKey("partnerid"))
            {
                PartnerId = int.Parse(RequestKeyValues["partnerid"]);
            }
        }
    }
}
