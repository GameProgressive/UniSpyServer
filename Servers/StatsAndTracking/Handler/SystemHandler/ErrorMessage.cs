using StatsAndTracking.Entity.Enumerator;

namespace StatsAndTracking.Handler.SystemHandler
{
    public class ErrorMessage
    {
        public static string ToMsg(GstatsErrorCode error)
        {
            switch (error)
            {
                case GstatsErrorCode.Parse:
                    return "Request parse error!";

                case GstatsErrorCode.General:
                    return "General error!";

                case GstatsErrorCode.Database:
                    return "Database error!";

                default:
                    return "Unknown error!";
            }
        }
    }
}
