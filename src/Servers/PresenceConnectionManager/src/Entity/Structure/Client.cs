using UniSpyServer.Servers.PresenceConnectionManager.Entity.Enumerate;
using UniSpyServer.Servers.PresenceConnectionManager.Structure;
using UniSpyServer.Servers.PresenceConnectionManager.Structure.Data;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure
{
    public class Client : ClientBase
    {
        public new ITcpSession Session => (ITcpSession)base.Session;
        public new ClientInfo Info { get => (ClientInfo)base.Info; private set => base.Info = value; }
        public Client(ISession session) : base(session)
        {
            Info = new ClientInfo(Session.RemoteIPEndPoint);
        }
        protected override void OnConnected()
        {
            base.OnConnected();
            // Only send the login challenge once
            if (Info.LoginPhase != LoginStatus.Connected)
            {
                Session.Disconnect();
                // Throw the error                
                LogWriter.Warning("The server challenge has already been sent. Cannot send another login challenge.");
            }

            Info.LoginPhase = LoginStatus.Processing;
            string sendingBuffer = $@"\lc\1\challenge\{LoginChallengeProof.ServerChallenge}\id\{1}\final\";
            LogWriter.LogNetworkSending(Session.RemoteIPEndPoint, sendingBuffer);
            Session.Send(sendingBuffer);
        }
    }
}