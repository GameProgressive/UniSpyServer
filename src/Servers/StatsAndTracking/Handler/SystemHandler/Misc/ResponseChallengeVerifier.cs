namespace StatsAndTracking.Handler.CommandHandler.SystemHandler.Misc
{
    public class ResponseChallengeVerifier
    {
        public static bool VerifyResponse(string response, uint connid)
        {
            uint temp = connid & 0x38F371E6;
            string connstr = temp.ToString();
            string result = "";

            for (int i = 0; i < connstr.Length; i++)
            {
                result += i + 17 + connstr[i];
            }

            return response == result ? true : false;
        }
    }
}
