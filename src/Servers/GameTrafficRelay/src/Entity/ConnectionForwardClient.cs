using UniSpyServer.Servers.GameTrafficRelay.Interface;
using UniSpyServer.UniSpyLib.Extensions;
using UniSpyServer.UniSpyLib.Logging;
namespace UniSpyServer.Servers.GameTrafficRelay.Entity.Structure
{
    public sealed class ConnectionForwardClient : IConnectionForwardClient
    {
        public IConnectionListener GameSpyTrafficListener { get; set; }
        public IConnectionForwardClient ForwardTargetClient { get; set; }
        public ConnectionForwardClient()
        {
        }
        public void ForwardMessage(byte[] data)
        {
            LogWriter.LogDebug($" [{GameSpyTrafficListener.ListeningEndPoint}] => [{ForwardTargetClient.GameSpyTrafficListener.ListeningEndPoint}] {StringExtensions.ConvertPrintableBytesToString(data)} [{StringExtensions.ConvertByteToHexString(data)}]");
            ForwardTargetClient.ForwardMessage(data);
        }
    }
}