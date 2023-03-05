using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Config;

namespace UniSpy.Server.Master.Application
{
    /// <summary>
    /// Master server is used for old games, and it function like the combination of QueryReport and ServerBrowser
    /// game server reports its data to Master server and game client retrive server list from Master server
    /// </summary>
    internal sealed class Server : UniSpy.Server.Core.Abstraction.BaseClass.Network.Tcp.Server.TcpServer
    {
        public Server(UniSpyServerConfig config) : base(config)
        {
        }
        protected override IClient CreateClient(IConnection connection) => new Client(connection);
    }
}