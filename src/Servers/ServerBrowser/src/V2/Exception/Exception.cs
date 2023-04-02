namespace UniSpy.Server.ServerBrowser.V2
{
    public class Exception : UniSpy.Exception
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
    }
}