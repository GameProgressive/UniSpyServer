namespace UniSpy.Server.QueryReport
{
    public sealed class Exception : UniSpy.Exception
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