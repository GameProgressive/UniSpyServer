using UniSpy.Server.PresenceConnectionManager.Enumerate;
using UniSpy.Server.PresenceConnectionManager.Handler;
using UniSpy.Server.PresenceConnectionManager.Structure;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.PresenceConnectionManager.Application
{
    public sealed class Client : ClientBase
    {
        public new ITcpConnection Connection => (ITcpConnection)base.Connection;
        public new ClientInfo Info { get => (ClientInfo)base.Info; private set => base.Info = value; }
        public Client(IConnection connection) : base(connection)
        {
            Info = new ClientInfo();
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

        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, buffer);
    }
}