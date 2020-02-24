using QueryReport.Entity.Enumerator;

namespace QueryReport.Handler.SystemHandler
{
    public class ErrorMessage
    {
        public static string ToMsg(QRErrorCode error)
        {
            switch (error)
            {
                case QRErrorCode.Parse:
                    return "Request parse error!";
                case QRErrorCode.General:
                    return "General error!";
                case QRErrorCode.Database:
                    return "Database error!";
                case QRErrorCode.Network:
                    return "Network error!";
                default:
                    return "Unknown error!";
            }
        }
    }
}
