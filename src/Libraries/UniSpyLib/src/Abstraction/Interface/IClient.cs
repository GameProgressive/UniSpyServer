using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.UniSpyLib.Abstraction.Interface
{
    public interface ITestClient
    {
        public void TestReceived(byte[] buffer);
    }
    public interface IClient
    {
        /// <summary>
        /// whether log raw request and response message bytes
        /// </summary>
        /// <value></value>
        bool IsLogRaw { get; }
        IConnection Connection { get; }
        ICryptography Crypto { get; }
        // we store client info here
        ClientInfoBase Info { get; }
        public void Send(IResponse response);
    }
}