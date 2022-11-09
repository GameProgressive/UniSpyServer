using UniSpyServer.Servers.PresenceConnectionManager.Entity.Enumerate;
using UniSpyServer.Servers.PresenceConnectionManager.Handler;
using UniSpyServer.Servers.PresenceConnectionManager.Structure;
using UniSpyServer.Servers.PresenceConnectionManager.Structure.Data;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure
{
    public class Client : ClientBase
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
            if (Info.LoginPhase != LoginStatus.Connected)
            {
                Connection.Disconnect();
                // Throw the error                
                LogWarn("The server challenge has already been sent. Cannot send another login challenge.");
            }

            Info.LoginPhase = LoginStatus.Processing;
            string sendingBuffer = $@"\lc\1\challenge\{LoginChallengeProof.ServerChallenge}\id\{1}\final\";
            LogNetworkSending(sendingBuffer);
            Connection.Send(sendingBuffer);
        }

        protected override ISwitcher CreateSwitcher(object buffer) => new CmdSwitcher(this, buffer);
    }
}