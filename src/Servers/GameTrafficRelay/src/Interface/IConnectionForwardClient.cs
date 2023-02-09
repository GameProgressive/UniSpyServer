namespace UniSpyServer.Servers.GameTrafficRelay.Interface
{
    public interface IConnectionForwardClient
    {
        IConnectionListener GameSpyTrafficListener { get; }
        IConnectionForwardClient ForwardTargetClient { get; }
        void ForwardMessage(byte[] data);
    }
}