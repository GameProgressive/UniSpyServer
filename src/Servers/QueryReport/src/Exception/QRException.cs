using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.QueryReport.Exception
{
    public sealed class QRException : UniSpyException, IResponse
    {
        public QRException()
        {
        }

        public QRException(string message) : base(message)
        {
        }

        public QRException(string message, System.Exception innerException) : base(message, innerException)
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