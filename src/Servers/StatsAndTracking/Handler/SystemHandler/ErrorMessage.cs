using StatsAndTracking.Entity.Enumerator;

namespace StatsAndTracking.Handler.SystemHandler
{
    public class ErrorMessage
    {
        public static string ToMsg(GStatsErrorCode error)
        {
            switch (error)
            {
                case GStatsErrorCode.Parse:
                    return "Request parse error!";

                case GStatsErrorCode.General:
                    return "General error!";

                case GStatsErrorCode.Database:
                    return "Database error!";

                default:
                    return "Unknown error!";
            }
        }
    }
}
