using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.Events;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.Core.Network.Tcp.Server
{
    public class TcpConnection : ITcpConnection
    {
        public IConnectionManager Manager { get; private set; }

        public IPEndPoint RemoteIPEndPoint { get; private set; }

        public NetworkConnectionType ConnectionType { get; } = NetworkConnectionType.Tcp;

        public event OnConnectedEventHandler OnConnect;
        public event OnDisconnectedEventHandler OnDisconnect;
        public event OnReceivedEventHandler OnReceive;

        public TcpClient Client { get; private set; }
        public TcpConnection(TcpClient client, IConnectionManager manager)
        {
            RemoteIPEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            Client = client;
            Manager = manager;
        }
        public void Disconnect()
        {
            Client.Client.Disconnect(true);
            OnDisconnected();
        }

        public void Send(string response) => Send(UniSpyEncoding.GetBytes(response));

        public void Send(byte[] response) => Client.Client.Send(response);

        public void OnConnected()
        {
            OnConnect();
            Task.Run(() => StartReceiving());
        }

        private void StartReceiving()
        {
            if (Client.Connected == false)
            {
                OnDisconnected();
                return;
            }
            var stream = Client.GetStream();
            byte[] buffer = new byte[2048];
            int bytesRead;
            while (true)
            {
                try
                {
                    if (!stream.CanRead || !stream.CanWrite)
                    {
                        break;
                    }
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        OnDisconnected();
                        break; // Connection closed by the client
                    }

                    var receivedData = buffer.Take(bytesRead).ToArray();
                    OnReceive(receivedData);
                }
                catch (SocketException ex)
                {
                    LogWriter.LogError(ex);
                    OnDisconnected();
                    break;
                }
            }
        }
        public void OnDisconnected()
        {
            OnDisconnect();
            Client.Close();
        }

        public void Dispose() => Client.Dispose();
    }
}