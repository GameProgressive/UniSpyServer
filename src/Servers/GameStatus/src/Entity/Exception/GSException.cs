using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.GameStatus.Entity.Exception
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