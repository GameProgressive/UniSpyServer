using UniSpyLib.Logging;
using UniSpyLib.Network;
using PresenceConnectionManager.Abstraction;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceConnectionManager.Entity.Structure;
using PresenceConnectionManager.Structure.Data;
using Serilog.Events;
using System;
using System.Net;
using PresenceConnectionManager.Handler.CommandSwitcher;

namespace PresenceConnectionManager.Network
{
    /// <summary>
    /// Gamespy Client Manager
    /// This class is used to proccess the client login process,
    /// create new user accounts, and fetch profile information
    /// <remarks>gpcm.gamespy.com</remarks>
    /// </summary>
    public class PCMSession : TCPSessionBase
    {
        /// <summary>
        /// Indicates whether this player successfully completed the login process
        /// </summary>
        public bool CompletedLoginProcess;

        /// <summary>
        /// Indicates the date and time this connection was created
        /// </summary>
        public readonly DateTime CreateTime;

        public UserData UserData;

        public PCMSession(TCPServerBase server) : base(server)
        {
            UserData = new UserData();
            CreateTime = new DateTime();
            CompletedLoginProcess = false;
        }

        protected override void OnConnected()
        {
            SendServerChallenge();
            base.OnConnected();
        }

        protected override void OnReceived(string message)
        {
            new PCMCommandSwitcher(this, message).Switch();
        }

        public void SendServerChallenge()
        {
            // Only send the login challenge once
            if (UserData.LoginStatus != LoginStatus.Connected)
            {
                Disconnect();
                // Throw the error                
                LogWriter.ToLog(
                    LogEventLevel.Warning,
                    "The server challenge has already been sent. Cannot send another login challenge.");
            }

            UserData.LoginStatus = LoginStatus.Processing;
            string sendingBuffer = $@"\lc\1\challenge\{ChallengeProofData.ServerChallenge}\id\{1}\final\";
            SendAsync(sendingBuffer);
        }

        public void StatusToLog(string status, string nick, uint pid, IPEndPoint remote, string reason)
        {
            string statusString = $@" [{status}] Nick:{nick}-PID:{pid}-IP:{remote}-Reason:{reason}";
            LogWriter.ToLog(LogEventLevel.Information, statusString);
        }
    }
}