using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.QueryReport
{
    public sealed class Exception : UniSpy.Exception, IResponse
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

        object IResponse.SendingBuffer => SendingBuffer;
        public string SendingBuffer { get; private set; }
        public void Build()
        {
            SendingBuffer = @$"\error\{Message}\final\";
        }
    }
}