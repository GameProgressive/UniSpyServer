using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.ServerBrowser
{
    public class Exception : UniSpy.Exception, IResponse
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

        public object SendingBuffer { get; private set; }

        public void Build()
        {
            SendingBuffer = @$"\error\{Message}\final\";
        }
    }
}