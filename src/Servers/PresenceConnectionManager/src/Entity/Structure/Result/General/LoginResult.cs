using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;

namespace UniSpy.Server.PresenceConnectionManager.Entity.Structure.Result
{
    public sealed class LogInDataModel
    {
        public int UserId;
        public int ProfileId;
        public string Nick;
        public string Email;
        public string UniqueNick;
        public string PasswordHash;
        public bool EmailVerifiedFlag;
        public bool BannedFlag;
        public int NamespaceId;
        public int SubProfileId;
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
