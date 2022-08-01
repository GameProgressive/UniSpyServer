using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.QueryReport.Entity.Exception
{
    public sealed class QRException : UniSpyException
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