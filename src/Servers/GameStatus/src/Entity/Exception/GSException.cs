using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.GameStatus.Entity.Exception
{
    public class GSException : UniSpyException
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