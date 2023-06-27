using UniSpy.Server.PresenceConnectionManager.Enumerate;
using UniSpy.Server.PresenceConnectionManager.Handler;
using UniSpy.Server.PresenceConnectionManager.Structure;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Logging;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.PresenceConnectionManager.Abstraction.Interface;
using UniSpy.Server.PresenceConnectionManager.Aggregate;

namespace UniSpy.Server.PresenceConnectionManager.Application
{
    public sealed class Client : ClientBase, IShareClient
    {
        public new ITcpConnection Connection => (ITcpConnection)base.Connection;
        public new ClientInfo Info { get => (ClientInfo)base.Info; set => base.Info = value; }
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
        protected override void OnConnected()
        {
            base.OnConnected();
            // Only send the login challenge once
            if (Info.LoginStat != LoginStatus.Connected)
            {
                Connection.Disconnect();
                // Throw the error                
                this.LogWarn("The server challenge has already been sent. Cannot send another login challenge.");
            }

            Info.LoginStat = LoginStatus.Processing;
            string sendingBuffer = $@"\lc\1\challenge\{LoginChallengeProof.ServerChallenge}\id\{1}\final\";
            this.LogNetworkSending(sendingBuffer);
            Connection.Send(sendingBuffer);
        }

        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, UniSpyEncoding.GetString((byte[])buffer));

        public RemoteClient GetRemoteClient()
        {
            _remoteClient.Info = Info;
            return _remoteClient;
        }
    }
}