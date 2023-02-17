using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.NatNegotiation.Exception
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