using System;
using System.Net;
using UniSpyServer.Servers.GameTrafficRelay.Interface;

namespace UniSpyServer.Servers.GameTrafficRelay.Entity.Structure
{
    public class ConnectionForwardBridge
    {
        public IConnectionForwardClient Client { get; private set; }
        public IConnectionListener Listener { get; private set; }
        public ConnectionForwardBridge(IConnectionListener listener, IConnectionForwardClient client)
        {
            Listener = listener;
            Client = client;
        }
    }
    /// <summary>
    /// This class represent a connection pair, 
    /// in this class 2 IConnectionForwardBridge instances is created and listen to 2 different avaliable udp port.
    /// The message send from gamespy client is redirect by the client in IConnectionForwardBridge
    /// </summary>
    public sealed class ConnectionPair
    {
        public uint Cookie { get; private set; }
        public IConnectionListener Listener1 { get; private set; }
        public IConnectionListener Listener2 { get; private set; }

        public ConnectionPair(IPEndPoint gameSpyClient1ListeningEnd, IPEndPoint gameSpyClient2ListeningEnd, uint cookie)
        {
            Cookie = cookie;
            Listener1 = new ConnectionListener(gameSpyClient1ListeningEnd, cookie);
            Listener2 = new ConnectionListener(gameSpyClient2ListeningEnd, cookie);
            // bind listener1 with client1
            Listener1.ForwardTargetListener = Listener2;
            Listener2.ForwardTargetListener = Listener1;
        }
    }
}