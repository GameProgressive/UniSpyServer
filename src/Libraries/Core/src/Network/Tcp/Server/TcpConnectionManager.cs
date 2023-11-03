using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Events;

namespace UniSpy.Server.Core.Network.Tcp.Server;

public class TcpConnectionManager : IConnectionManager, IDisposable
{
    public event OnConnectingEventHandler OnInitialization;
    public TcpListener Listener;
    public TcpConnectionManager(IPEndPoint endPoint)
    {
        Listener = new TcpListener(endPoint);
    }

    public void Start()
    {
        Listener.Start();
        Task.Run(() =>
        {
            while (true)
            {
                var client = Listener.AcceptTcpClient();
                var conn = new TcpConnection(client, this);
                Task.Run(() =>
                {
                    OnInitialization(conn);
                    conn.OnConnected();
                });
            }
        });
    }

    public void Dispose()
    {
        Listener.Stop();
    }
}