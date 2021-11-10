using UniSpyServer.Servers.GameStatus.Entity.Enumerate;

namespace UniSpyServer.Servers.GameStatus.Handler.SystemHandler
{
    public sealed class ErrorMessage
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
