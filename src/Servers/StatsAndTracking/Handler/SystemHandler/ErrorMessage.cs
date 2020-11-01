using StatsTracking.Entity.Enumerate;

namespace StatsTracking.Handler.SystemHandler
{
    public class ErrorMessage
    {
        public static string ToMsg(STError error)
        {
            switch (error)
            {
                case STError.Parse:
                    return "Request parse error!";

                case STError.General:
                    return "General error!";

                case STError.Database:
                    return "Database error!";

                default:
                    return "Unknown error!";
            }
        }
    }
}
