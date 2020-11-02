using GameStatus.Entity.Enumerate;

namespace GameStatus.Handler.SystemHandler
{
    public class ErrorMessage
    {
        public static string ToMsg(GSError error)
        {
            switch (error)
            {
                case GSError.Parse:
                    return "Request parse error!";

                case GSError.General:
                    return "General error!";

                case GSError.Database:
                    return "Database error!";

                default:
                    return "Unknown error!";
            }
        }
    }
}
