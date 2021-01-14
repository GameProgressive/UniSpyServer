using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Structure.Data;

namespace PresenceConnectionManager.Entity.Structure.Result
{
    internal class LogInDataModel
    {
        public uint UserID;
        public uint ProfileID;
        public string Nick;
        public string Email;
        public string UniqueNick;
        public string PasswordHash;
        public bool EmailVerifiedFlag;
        public bool BannedFlag;
        public uint NamespaceID;
        public uint SubProfileID;
    }

    internal class LogInResult : PCMResultBase
    {
        public LogInDataModel DatabaseResults { get; set; }
        public string ResponseProof { get; set; }
        public PCMUserInfo UserInfo { get; set; }
        public LogInResult()
        {
        }
    }
}
