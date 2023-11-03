using System.Net;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.Events;

namespace UniSpy.Server.Core.Network.Udp.Server;

public class UdpConnection : IUdpConnection
{
    public IConnectionManager Manager { get; private set; }

    public IPEndPoint RemoteIPEndPoint { get; private set; }

    public NetworkConnectionType ConnectionType { get; } = NetworkConnectionType.Udp;

    public event OnReceivedEventHandler OnReceive;

    public UdpConnection(IPEndPoint endPoint, IConnectionManager manager)
    {
        RemoteIPEndPoint = endPoint;
        Manager = manager;
    }
    public void OnReceived(byte[] buffer)
    {
        OnReceive(buffer);
    }
    public void Send(IPEndPoint endPoint, byte[] response)
    {
        (Manager as UdpConnectionManager).Listener.Send(response, response.Length, endPoint);
    }

    public void Send(IPEndPoint endPoint, string response)
    {
        Send(endPoint, UniSpyEncoding.GetBytes(response));
    }

    public void Send(string response)
    {
        Send(UniSpyEncoding.GetBytes(response));
    }

    public void Send(byte[] response)
    {
        (Manager as UdpConnectionManager).Listener.Send(response, response.Length, RemoteIPEndPoint);
    }

    public override bool Equals(object obj)
    {
        return RemoteIPEndPoint.Equals((obj as UdpConnection).RemoteIPEndPoint);
    }
    public override int GetHashCode()
    {
        return RemoteIPEndPoint.GetHashCode();
    }
}