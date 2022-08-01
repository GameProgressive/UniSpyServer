using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Exception
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