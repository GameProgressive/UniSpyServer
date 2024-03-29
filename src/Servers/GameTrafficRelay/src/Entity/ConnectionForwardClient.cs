using UniSpy.Server.GameTrafficRelay.Interface;
using UniSpy.Server.Core.Extension;
using UniSpy.Server.Core.Logging;
namespace UniSpy.Server.GameTrafficRelay.Aggregate
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