namespace UniSpy.Server.Chat
{
    public class Exception : UniSpy.Exception
    {
        public Exception() { }

        public Exception(string message) : base(message) { }

        public Exception(string message, System.Exception innerException) : base(message, innerException) { }
    }

    public class HandleLaterException : Exception
    {
        public HandleLaterException(string message) : base(message) { }
    }
}