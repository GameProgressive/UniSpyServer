using System.Collections.Generic;
using System.Threading.Tasks;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Config;
using UniSpy.Server.Core.Database.DatabaseModel;

namespace UniSpy.Server.ServerBrowser.Application
{
    internal sealed class Server : UniSpy.Server.Core.Abstraction.BaseClass.Network.Tcp.Server.TcpServer
    {
        public static readonly Dictionary<string, List<Grouplist>> PeerGroupList = StorageOperation.Persistance.GetAllGroupList();
        public Server(UniSpyServerConfig config) : base(config)
        {
        }
        public override void Start()
        {
            StorageOperation.HeartbeatChannel.StartSubscribe();
            base.Start();
        }
        protected override IClient CreateClient(IConnection connection) => new Client(connection);
    }
}