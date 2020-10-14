using PresenceConnectionManager.Entity.Enumerator;
using System;

namespace PresenceConnectionManager.Entity.Structure
{
    public class ChallengeProofData
    {
        public const string ServerChallenge = "0000000000";
        public string UserData { get; protected set; }
        public LoginType LoginType { get; protected set; }
        public uint PartnerID { get; protected set; }
        public string Challenge1 { get; protected set; }
        public string Challenge2 { get; protected set; }
        public string PasswordHash { get; protected set; }

        public ChallengeProofData(string userData, LoginType loginType, uint partnerID, string challenge1, string challenge2, string passwordHash)
        {
            UserData = userData ?? throw new ArgumentNullException(nameof(userData));
            LoginType = loginType;
            PartnerID = partnerID;
            Challenge1 = challenge1 ?? throw new ArgumentNullException(nameof(challenge1));
            Challenge2 = challenge2 ?? throw new ArgumentNullException(nameof(challenge2));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        }
    }
}
