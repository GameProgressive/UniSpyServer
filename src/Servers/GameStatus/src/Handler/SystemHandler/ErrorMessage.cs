using GameStatus.Entity.Enumerate;

namespace GameStatus.Handler.SystemHandler
{
    internal sealed class ErrorMessage
    {
        public static string ToMsg(GSErrorCode error)
        {
            switch (error)
            {
                case GSErrorCode.Parse:
                    return "Request parse error!";

                case GSErrorCode.General:
                    return "General error!";

                case GSErrorCode.Database:
                    return "Database error!";

                default:
                    return "Unknown error!";
            }
        }
    }
}
