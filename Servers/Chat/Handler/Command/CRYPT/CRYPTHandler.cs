using GameSpyLib.Common;


namespace Chat.Handler.CRYPT
{
    public class CRYPTHandler
    {
        public static void Handle()
        {
            //this is a fake response;
            string clientKey = GameSpyRandom.GenerateRandomString(16, GameSpyRandom.StringType.AlphaNumeric);
            string serverKey = GameSpyRandom.GenerateRandomString(16, GameSpyRandom.StringType.AlphaNumeric);

        }
    }
}
