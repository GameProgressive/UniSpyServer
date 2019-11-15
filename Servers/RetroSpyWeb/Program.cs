using Handler.AuthHandler.Program;

namespace RetroSpyWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AuthServiceCreator.CreateHTTPAuthService();
        }

    }
}
