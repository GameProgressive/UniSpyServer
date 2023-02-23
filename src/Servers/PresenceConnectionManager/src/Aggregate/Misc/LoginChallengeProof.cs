using System.Text;
using UniSpy.Server.PresenceConnectionManager.Enumerate;
using UniSpy.Server.Core.Extension;

namespace UniSpy.Server.PresenceConnectionManager.Structure
{
    public sealed class LoginChallengeProof
    {
        public const string ServerChallenge = "0000000000";
        public string UserData { get; private set; }
        public LoginType LoginType { get; private set; }
        public int? PartnerID { get; private set; }
        public string Challenge1 { get; private set; }
        public string Challenge2 { get; private set; }
        public string PasswordHash { get; private set; }

        public LoginChallengeProof(string userData, LoginType loginType, int? partnerID, string challenge1, string challenge2, string passwordHash)
        {
            UserData = userData;
            LoginType = loginType;
            PartnerID = partnerID;
            Challenge1 = challenge1;
            Challenge2 = challenge2;
            PasswordHash = passwordHash;
        }

        /// <summary>
        /// Generates an MD5 hash, which is used to verify the connections login information
        /// </summary>
        /// <returns>
        ///     The proof verification MD5 hash string that can be compared to what the _connection sends,
        ///     to verify that the users entered password matches the specific user data in the database.
        /// </returns>
        public static string GenerateProof(LoginChallengeProof data)
        {
            string tempUserData = data.UserData;

            // Login types NickEmail and AuthToken don't use partnerID append
            if (data.PartnerID is not null)
            {
                if (data?.PartnerID != (int)GPPartnerID.Gamespy
                             && data?.LoginType != LoginType.AuthToken)
                {
                    tempUserData = $@"{data.PartnerID}@{data.UserData}";
                }
            }

            // Generate our response string
            StringBuilder responseString = new StringBuilder(data.PasswordHash);
            responseString.Append(' ', 48); // 48 spaces
            responseString.Append(tempUserData);
            responseString.Append(data.Challenge1);
            responseString.Append(data.Challenge2);
            responseString.Append(data.PasswordHash);

            string hashString = responseString.ToString().GetMD5Hash();

            return hashString;
        }
    }
}
