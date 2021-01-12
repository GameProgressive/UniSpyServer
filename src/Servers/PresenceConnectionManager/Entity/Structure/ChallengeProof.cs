using UniSpyLib.Extensions;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceConnectionManager.Entity.Structure;
using System.Text;

namespace PresenceConnectionManager.Structure
{
    internal class ChallengeProof
    {
        /// <summary>
        /// Generates an MD5 hash, which is used to verify the sessions login information
        /// </summary>
        /// <returns>
        ///     The proof verification MD5 hash string that can be compared to what the _session sends,
        ///     to verify that the users entered password matches the specific user data in the database.
        /// </returns>
        public static string GenerateProof(ChallengeProofData data)
        {
            string tempUserData = data.UserData;

            // Auth token does not have partnerid append.
            if (data.PartnerID != (uint)PartnerID.Gamespy && data.LoginType != LoginType.AuthToken)
            {
                tempUserData = $@"{data.PartnerID}@{data.UserData}";
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
