using UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Contract;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Request
{
    [RequestContract("check")]
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

            if (RequestKeyValues.ContainsKey("partnerid"))
            {
                PartnerId = int.Parse(RequestKeyValues["partnerid"]);
            }
        }
    }
}
