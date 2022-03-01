using UniSpyServer.Servers.PresenceConnectionManager.Entity.Enumerate;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Misc
{
    public sealed class BasicInfo
    {
        public int? UserId { get; set; }
        public int? ProfileId { get; set; }
        public int? SubProfileId { get; set; }
        public string Nick { get; set; }
        public string UniqueNick { get; set; }
        public string Email { get; set; }
        public int? PartnerId { get; set; }
        public int? NamespaceId { get; set; }
        public int? ProductId { get; set; }
        public int? GamePort { get; set; }
        public string GameName { get; set; }
        /// <summary>
        /// The connected clients Authentication Token
        /// </summary>
        public string AuthToken { get; set; }

        /// <summary>
        /// Indicates the user's Login process status.
        /// </summary>
        public LoginStatus LoginStatus { get; set; }
        public GPBasic QuietModeFlag { get; set; }
        public SdkRevisionType SdkType { get; set; }
        public BasicInfo()
        {
            LoginStatus = LoginStatus.Connected;
        }
    }
}
