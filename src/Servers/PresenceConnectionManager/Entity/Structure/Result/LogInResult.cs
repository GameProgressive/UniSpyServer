using System;
using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Request;
using PresenceConnectionManager.Structure.Data;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Result
{
    public class LogInDataModel
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

    public class LogInResult : PCMResultBase
    {
        public LogInDataModel DatabaseResults { get; set; }
        public string ResponseProof { get; set; }
        public PCMUserInfo UserInfo { get; set; }
        public new LoginRequest Request => (LoginRequest)base.Request;
        public LogInResult(UniSpyRequestBase request) : base(request)
        {
        }

        public LogInResult()
        {
        }
    }
}
