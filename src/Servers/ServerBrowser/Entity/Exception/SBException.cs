using UniSpyLib.Abstraction.BaseClass;

namespace ServerBrowser.Entity.Exception
{
    public class SBException : UniSpyExceptionBase
    {
        public SBException()
        {
        }

        public SBException(string message) : base(message)
        {
        }

        public SBException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}