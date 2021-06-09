using System;
using System.Net;
using System.Net.Sockets;
using UdpClient = NetCoreServer.UdpClient;

namespace UniSpyLib.Network.Udp
{
    public class UDPClientBase : UdpClient
    {

        public UDPClientBase(IPAddress address, int port) : base(address, port)
        {
        }
        protected override void OnConnected()
        {
            Console.WriteLine($"Echo UDP client connected a new session with Id {Id}");

            // Start receive datagrams
            ReceiveAsync();

        }
        protected override void OnDisconnected()
        {
            Console.WriteLine($"Echo UDP client disconnected a session with Id {Id}");
            base.OnConnected();
        }

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            // Continue receive datagrams
            ReceiveAsync();
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Echo UDP client caught an error with code {error}");
        }

    }
}
