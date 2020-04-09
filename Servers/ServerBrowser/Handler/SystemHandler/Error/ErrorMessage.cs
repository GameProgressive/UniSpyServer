using ServerBrowser.Entity.Enumerator;

namespace ServerBrowser.Handler.SystemHandler.Error
{
    public class ErrorMessage
    {
        public static string GetErrorMsg(SBErrorCode code)
        {
            switch (code)
            {
                case SBErrorCode.Parse:
                    return "Parsing error";
                case SBErrorCode.DataOperation:
                    return "Data operation error";
                case SBErrorCode.NoServersFound:
                    return "Can not find online server";
                case SBErrorCode.UnSupportedGame:
                    return "The game is not supported by RetroSpy";
                default:
                    return "Unknown error";
            }
        }
    }
}
