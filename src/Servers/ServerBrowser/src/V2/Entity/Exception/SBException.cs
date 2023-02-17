using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.ServerBrowser.V2.Entity.Exception
{
    public class SBException : UniSpyException
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