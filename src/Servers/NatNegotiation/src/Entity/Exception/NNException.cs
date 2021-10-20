using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.NatNegotiation.Entity.Exception
{
    public class NNException : UniSpyException
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