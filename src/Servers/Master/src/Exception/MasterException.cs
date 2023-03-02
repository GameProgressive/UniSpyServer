using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.Master
{
    public class Exception : UniSpyException
    {
        public Exception()
        {
        }

        public Exception(string message) : base(message)
        {
        }

        public Exception(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}