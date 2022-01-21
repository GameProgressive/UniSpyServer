using UniSpyServer.Servers.PresenceConnectionManager.Entity.Enumerate;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Misc
{
    public sealed class PCMBasicInfo
    {
        public int UserID { get; set; }
        public int ProfileId { get; set; }
        public int SubProfileID { get; set; }
        public string Nick { get; set; }
        public string UniqueNick { get; set; }
        public string Email { get; set; }
        public int PartnerID { get; set; }
        public int NamespaceID { get; set; }
        public int ProductID { get; set; }
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
        public SDKRevisionType SDKRevision { get; set; }
        public PCMBasicInfo()
        {
            LoginStatus = LoginStatus.Connected;
        }
    }
}
