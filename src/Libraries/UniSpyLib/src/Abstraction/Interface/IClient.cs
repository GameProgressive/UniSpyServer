using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.UniSpyLib.Abstraction.Interface
{
    public interface ITestClient
    {
        public void TestReceived(byte[] buffer);
    }
    public interface IClient : ILogger
    {
        // we store client info here
        IConnection Connection { get; }
        ICryptography Crypto { get; }
        ClientInfoBase Info { get; }
        public void Send(IResponse response);
    }
}