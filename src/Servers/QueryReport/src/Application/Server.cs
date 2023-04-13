using System.Net;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Network.Udp.Server;
using UniSpy.Server.QueryReport.V2.Application;

namespace UniSpy.Server.QueryReport.Application
{
    public sealed class Server : ServerBase
    {
        static Server()
        {
            _name = "QueryReport";
        }
        public Server() { }

        public Server(IConnectionManager manager) : base(manager) { }

        public override void Start()
        {
            base.Start();
            V2.Application.StorageOperation.NatNegChannel.StartSubscribe();
            // V2.Application.StorageOperation.Persistance.InitPeerRoomsInfo();
        }
        protected override IClient CreateClient(IConnection connection) => new Client(connection, this);

        protected override IConnectionManager CreateConnectionManager(IPEndPoint endPoint) => new UdpConnectionManager(endPoint);

    }
}