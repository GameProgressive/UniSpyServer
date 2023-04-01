using System.Linq;
using System.Threading.Tasks;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Aggregate;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Aggregate.Redis.Contract;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Handler;
using UniSpy.Server.Chat.Handler.CmdHandler.General;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.Chat.Application
{
    public class Client : ClientBase, IChatClient
    {
        public new ClientInfo Info { get => (ClientInfo)base.Info; private set => base.Info = value; }
        public new ITcpConnection Connection => (ITcpConnection)base.Connection;
        private BufferCache _bufferCache = new BufferCache();
        public Client(IConnection connection, IServer server) : base(connection, server)
        {
            Info = new ClientInfo();
        }

        protected override void OnReceived(object buffer)
        {
            var message = DecryptMessage((byte[])buffer);
            if (_bufferCache.ProcessBuffer(message, out var completeBuffer))
            {
                this.LogNetworkReceiving(completeBuffer);
                var switcher = CreateSwitcher(completeBuffer);
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    switcher.Switch();
                }
                else
                {
                    Task.Run(() => switcher.Switch());
                }
            }
        }
        protected override void OnDisconnected()
        {
            if (Info.IsLoggedIn)
            {
                var req = new QuitRequest()
                {
                    Reason = $"{Info.NickName} Disconnected."
                };
                new QuitHandler(this, req).Handle();
                Info.IsLoggedIn = false;
            }
            Task.Run(() => PublishDisconnectMessage());
            base.OnDisconnected();
        }
        private void PublishDisconnectMessage()
        {
            var message = new RemoteMessage(new DisconnectRequest(), GetRemoteClient());
            Chat.Application.Server.GeneralChannel.PublishMessage(message);
        }
        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, buffer);
        public RemoteClient GetRemoteClient()
        {
            var manager = new RemoteTcpConnectionManager();
            var conn = new RemoteTcpConnection(Connection, manager);
            var server = new RemoteServer(Server);
            var client = new RemoteClient(conn, Info, server);
            return client;
        }
    }
}