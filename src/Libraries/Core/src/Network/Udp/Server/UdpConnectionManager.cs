using System.Collections.Concurrent;
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
    public ConcurrentDictionary<IPEndPoint, UdpConnection> Pool = new();
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
                var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                var data = Listener.Receive(ref clientEndPoint);

                Task.Run(() =>
                        {
                            UdpConnection conn;
                            if (!Pool.TryGetValue(clientEndPoint, out conn))
                            {
                                conn = new UdpConnection(clientEndPoint, this);
                                Pool.TryAdd(clientEndPoint, conn);
                            }
                            OnInitialization(conn);
                            conn.OnReceived(data);
                        });
            }
        });
    }


    public void Dispose()
    {
        Listener.Dispose();
    }
}