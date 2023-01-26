using System.Net;

namespace UniSpyServer.Servers.GameTrafficRelay.Interface
{
    public interface IConnectionListener
    {
        IPEndPoint ListeningEndPoint { get; }
        // IConnectionForwardClient Client { get; }
        IPEndPoint GameSpyClientIPEndPoint { get; }
        public IConnectionListener ForwardTargetListener { get; set; }

        void ForwardMessage(byte[] data);
        void Dispose();
    }
}