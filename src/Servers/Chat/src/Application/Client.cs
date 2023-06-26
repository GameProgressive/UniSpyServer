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
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.Chat.Application
{
    public class Client : ClientBase, IShareClient
    {
        public new ClientInfo Info { get => (ClientInfo)base.Info; private set => base.Info = value; }
        public new ITcpConnection Connection => (ITcpConnection)base.Connection;
        private BufferCache _bufferCache = new BufferCache();
        private RemoteClient _remoteClient;
        public Client(IConnection connection, IServer server) : base(connection, server)
        {
            Info = new ClientInfo();
            _remoteClient = new RemoteClient(this);
        }
        protected override void OnConnected()
        {
            Info.IsRemoteClient = false;
            base.OnConnected();
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
                    switcher.Handle();
                }
                else
                {
                    Task.Run(() => switcher.Handle());
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
            var request = new QuitRequest() { Reason = "Disconnect from server" };
            var message = new RemoteMessage(request, GetRemoteClient());
            Chat.Application.Server.GeneralChannel.PublishMessage(message);
            base.OnDisconnected();
        }
        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, UniSpyEncoding.GetString((byte[])buffer));
        public RemoteClient GetRemoteClient()
        {
            _remoteClient.Info = Info;
            return _remoteClient;
        }
    }
}