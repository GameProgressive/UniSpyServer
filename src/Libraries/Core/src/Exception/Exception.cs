using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Logging;

namespace UniSpy
{
    public class Exception : System.Exception
    {
        public Exception()
        {
        }

        public Exception(string message) : base(message)
        {
        }

        public Exception(string message, System.Exception innerException) : base(message, innerException)
        {
        }
        public static void HandleException(System.Exception ex, IClient client = null)
        {
            // we only log exception message when this message is UniSpy.Exception
            if (ex is UniSpy.Exception)
            {
                if (client is null)
                {
                    LogWriter.LogError(ex.Message);
                }
                else
                {
                    client.LogError(ex.Message);
                }
            }
            else
            {
                if (client is null)
                {
                    LogWriter.LogError(ex.ToString());
                }
                else
                {
                    client.LogError(ex.ToString());
                }
            }
        }
    }

}
