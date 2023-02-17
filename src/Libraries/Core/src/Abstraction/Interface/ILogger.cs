using System.Net;

namespace UniSpy.Server.Core.Abstraction.Interface
{
    public interface ILogger
    {
        public void LogInfo(string message);
        public void LogVerbose(string message);
        public void LogDebug(string message);
        public void LogWarn(string messsage);
        public void LogError(string message);
        public void LogNetworkReceiving(byte[] message);
        public void LogNetworkReceiving(string message);
        public void LogNetworkSending(byte[] message);
        public void LogNetworkSending(string message);
        public void LogNetworkForwarding(IPEndPoint receiver, byte[] message);
        public void LogNetworkForwarding(IPEndPoint receiver, string message);
        public void LogNetworkMultiCast(byte[] message);
        public void LogNetworkMultiCast(string message);
    }
}