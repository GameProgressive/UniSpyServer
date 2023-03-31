using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.Core.Abstraction.Interface
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
        IServer Server { get; }
        ICryptography Crypto { get; }
        // we store client info here
        ClientInfoBase Info { get; }
        public void Send(IResponse response);
    }
}