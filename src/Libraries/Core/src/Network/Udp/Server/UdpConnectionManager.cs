using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Events;

namespace UniSpy.Server.Core.Network.Udp.Server;

public class UdpConnectionManager : IConnectionManager, IDisposable
{
    public event OnConnectingEventHandler OnInitialization;
    public UdpClient Listener { get; private set; }
    public UdpConnectionManager(IPEndPoint endPoint)
    {
        Listener = new UdpClient(endPoint);
    }

    public void Start()
    {
        Task.Run(() =>
        {
            while (true)
            {
                var clientEndPoint = new IPEndPoint(IPAddress.Any, (Listener.Client.RemoteEndPoint as IPEndPoint).Port);
                var data = Listener.Receive(ref clientEndPoint);
                var conn = new UdpConnection(clientEndPoint, this);
                OnConnecting(conn);
                Task.Run(() => conn.OnReceived(data));
            }
        });
    }
    public void OnConnecting(IUdpConnection connection)
    {
        OnInitialization?.Invoke(connection);
    }

    public void Dispose()
    {
        Listener.Dispose();
    }
}