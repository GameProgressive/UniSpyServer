using PresenceConnectionManager.Entity.Enumerate;

namespace PresenceConnectionManager.Entity.Structure.Misc
{
    internal sealed class PCMBasicInfo
    {
        public uint UserID { get; set; }
        public uint ProfileID { get; set; }
        public uint SubProfileID { get; set; }
        public string Nick { get; set; }
        public string UniqueNick { get; set; }
        public string Email { get; set; }
        public uint PartnerID { get; set; }
        public uint NamespaceID { get; set; }
        public uint ProductID { get; set; }
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
