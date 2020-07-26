using System;
using PresenceConnectionManager.Entity.Enumerator;

namespace PresenceConnectionManager.Entity.Structure
{
    public class ChallengeProofData
    {
        public const string ServerChallenge = "0000000000";
        public string UserData { get; protected set; }
        public LoginType LoginType { get; protected set; }
        public uint PartnerID { get; protected set; }
        public string UserChallenge { get; protected set; }
        public string PasswordHash { get; protected set; }

        public ChallengeProofData(string userData, LoginType loginType, uint partnerID, string userChallenge, string passwordHash)
        {
            UserData = userData ?? throw new ArgumentNullException(nameof(userData));
            LoginType = loginType;
            PartnerID = partnerID;
            UserChallenge = userChallenge ?? throw new ArgumentNullException(nameof(userChallenge));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        }
    }
}
