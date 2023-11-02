using System;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Aggregate;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Handler;
using UniSpy.Server.Chat.Handler.CmdHandler.General;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.Extension;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.Chat.Application
{
    public class Client : ClientBase, IShareClient
    {
        public new ClientInfo Info { get => (ClientInfo)base.Info; private set => base.Info = value; }
        public new ITcpConnection Connection => (ITcpConnection)base.Connection;
        public bool IsRemoteClient => !ClientManager.ClientPool.ContainsKey(Connection.RemoteIPEndPoint);
        private RemoteClient _remoteClient;
        public Client(IConnection connection, IServer server) : base(connection, server)
        {
            Info = new ClientInfo();
            _remoteClient = new RemoteClient(this);
        }
        public Client(IConnection connection, IServer server, ClientInfo info) : this(connection, server)
        {
            Info = info;
            _remoteClient = new RemoteClient(this);
        }
        protected override void EventBinding()
        {
            base.EventBinding();
            // bind a event that can update the client info to redis
            _timer = new EasyTimer(TimeSpan.FromMinutes(1));
            _timer.Elapsed += (s, e) => Application.StorageOperation.Persistance.UpdateClient(this);
            _timer.Start();
        }
        protected override void OnReceived(object buffer)
        {
            var message = DecryptMessage((byte[])buffer);
            this.LogNetworkReceiving(message);
            var switcher = CreateSwitcher(message);
            switcher.Handle();
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
            StorageOperation.Persistance.RemoveClient(this);
            base.OnDisconnected();
        }
        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, UniSpyEncoding.GetString((byte[])buffer));
        public RemoteClient GetRemoteClient() => _remoteClient;
    }
}