using UniSpyLib.Abstraction.BaseClass;

namespace QueryReport.Entity.Exception
{
    internal sealed class QRException : UniSpyExceptionBase
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
    }
}