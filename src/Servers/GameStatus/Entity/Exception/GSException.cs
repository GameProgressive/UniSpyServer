using UniSpyLib.Abstraction.BaseClass;

namespace GameStatus.Entity.Exception
{
    public class GSException : UniSpyExceptionBase
    {
        public GSException()
        {
        }

        public GSException(string message) : base(message)
        {
        }

        public GSException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}