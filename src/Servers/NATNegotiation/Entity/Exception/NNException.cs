using UniSpyLib.Abstraction.BaseClass;

namespace NATNegotiation.Entity.Exception
{
    public class NNException : UniSpyExceptionBase
    {
        public NNException()
        {
        }

        public NNException(string message) : base(message)
        {
        }

        public NNException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}