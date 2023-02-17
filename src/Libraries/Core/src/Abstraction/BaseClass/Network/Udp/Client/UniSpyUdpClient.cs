using System;
using System.Net;
using System.Net.Sockets;
using UdpClient = NetCoreServer.UdpClient;

namespace UniSpy.Server.Core.Abstraction.BaseClass.Network.Udp.Client
{
    public class UniSpyUdpClient : UdpClient
    {

        public UniSpyUdpClient(IPAddress address, int port) : base(address, port)
        {
        }
        protected override void OnConnected()
        {
            Console.WriteLine($"Echo UDP client connected a new connection with Id {Id}");

            // Start receive datagrams
            ReceiveAsync();
        }
        protected override void OnDisconnected()
        {
            Console.WriteLine($"Echo UDP client disconnected a connection with Id {Id}");
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
