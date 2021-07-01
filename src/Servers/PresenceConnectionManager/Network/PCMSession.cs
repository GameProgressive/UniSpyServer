using PresenceConnectionManager.Entity.Enumerate;
using PresenceConnectionManager.Handler.CommandSwitcher;
using PresenceConnectionManager.Structure;
using PresenceConnectionManager.Structure.Data;
using Serilog.Events;
using UniSpyLib.Logging;
using UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;
namespace PresenceConnectionManager.Network
{
    /// <summary>
    /// Gamespy Client Manager
    /// This class is used to proccess the client login process,
    /// create new user accounts, and fetch profile information
    /// <remarks>gpcm.gamespy.com</remarks>
    /// </summary>
    internal sealed class PCMSession : UniSpyTcpSession
    {
        /// <summary>
        /// Indicates the date and time this connection was created
        /// </summary>
        public PCMUserInfo UserInfo;
        public PCMSession(UniSpyTcpServer server) : base(server)
        {
            UserInfo = new PCMUserInfo(Id);
        }

        protected override void OnConnected()
        {
            SendServerChallenge();
            base.OnConnected();
        }
        protected override void OnReceived(string message) => new PCMCmdSwitcher(this, message).Switch();
        public void SendServerChallenge()
        {
            // Only send the login challenge once
            if (UserInfo.BasicInfo.LoginStatus != LoginStatus.Connected)
            {
                Disconnect();
                // Throw the error                
                LogWriter.ToLog(
                    LogEventLevel.Warning,
                    "The server challenge has already been sent. Cannot send another login challenge.");
            }

            UserInfo.BasicInfo.LoginStatus = LoginStatus.Processing;
            string sendingBuffer = $@"\lc\1\challenge\{LoginChallengeProof.ServerChallenge}\id\{1}\final\";
            SendAsync(sendingBuffer);
        }
    }
}