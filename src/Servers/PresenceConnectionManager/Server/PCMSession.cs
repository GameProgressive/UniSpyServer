using GameSpyLib.Logging;
using GameSpyLib.Network;
using PresenceConnectionManager.Entity.Enumerator;
using PresenceConnectionManager.Entity.Structure;
using PresenceConnectionManager.Handler;
using PresenceConnectionManager.Structure.Data;
using Serilog.Events;
using System;
using System.Net;

namespace PresenceConnectionManager
{
    /// <summary>
    /// Gamespy Client Manager
    /// This class is used to proccess the client login process,
    /// create new user accounts, and fetch profile information
    /// <remarks>gpcm.gamespy.com</remarks>
    /// </summary>
    public class PCMSession : TemplateTcpSession
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

        public PCMSession(TemplateTcpServer server) : base(server)
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
            PCMCommandSwitcher.Switch(this, message);

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
            string sendingBuffer = string.Format(@"\lc\1\challenge\{0}\id\{1}\final\", ChallengeProofData.ServerChallenge, 1);
            SendAsync(sendingBuffer);
        }

        public void StatusToLog(string status, string nick, uint pid, IPEndPoint remote, string reason)
        {
            string statusString = string.Format(@" [{0}] Nick:{1}-PID:{2}-IP:{3}-Reason:{4}", status, nick, pid, remote, reason);
            LogWriter.ToLog(LogEventLevel.Information, statusString);
        }
    }
}