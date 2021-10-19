using PresenceConnectionManager.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Result
{
    public sealed class LogInDataModel
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

    public sealed class LoginResult : ResultBase
    {
        public LogInDataModel DatabaseResults { get; set; }
        public string ResponseProof { get; set; }
        public LoginResult()
        {
        }
    }
}
