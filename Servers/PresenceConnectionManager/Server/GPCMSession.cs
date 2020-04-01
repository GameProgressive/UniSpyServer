using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler;
using PresenceConnectionManager.Structure;
using System;
using System.Collections.Generic;
using System.Net;

namespace PresenceConnectionManager
{
    /// <summary>
    /// Gamespy Client Manager
    /// This class is used to proccess the client login process,
    /// create new user accounts, and fetch profile information
    /// <remarks>gpcm.gamespy.com</remarks>
    /// </summary>
    public class GPCMSession : TemplateTcpSession
    {
        /// <summary>
        /// Indicates whether this player successfully completed the login process
        /// </summary>
        public bool CompletedLoginProcess { get; set; } = false;

        /// <summary>
        /// Indicates the date and time this connection was created
        /// </summary>
        public readonly DateTime Created = DateTime.Now;

        public UserInfo UserInfo = new UserInfo();

        public GPCMSession(TemplateTcpServer server) : base(server)
        {
        }

        protected override void OnReceived(string message)
        {
            message = RequstFormatConversion(message);

            if (message[0] != '\\')
            {
                GameSpyUtils.SendGPError(this, GPErrorCode.General, "An invalid request was sended.");
                return;
            }

            string[] commands = message.Split("\\final\\");

            foreach (string command in commands)
            {
                if (command.Length < 1)
                {
                    continue;
                }

                // Read client message, and parse it into key value pairs
                string[] recieved = command.TrimStart('\\').Split('\\');

                Dictionary<string, string> dict = GameSpyUtils.ConvertRequestToKeyValue(recieved);

                CommandSwitcher.Switch(this, dict);
            }
        }



        public void SendServerChallenge()
        {
            // Only send the login challenge once
            if (UserInfo.LoginProcess != LoginStatus.Connected)
            {
                Disconnect();
                // Throw the error                
                ToLog(Serilog.Events.LogEventLevel.Warning, "The server challenge has already been sent. Cannot send another login challenge.");
            }

            // We send the client the challenge key
            string serverChallengeKey = GameSpyRandom.GenerateRandomString(10, GameSpyLib.Common.GameSpyRandom.StringType.Alpha);
            UserInfo.ServerChallenge = serverChallengeKey;
            UserInfo.LoginProcess = LoginStatus.Processing;
            string sendingBuffer = string.Format(@"\lc\1\challenge\{0}\id\{1}\final\", serverChallengeKey, 1);
            SendAsync(sendingBuffer);
        }

        public void StatusToLog(string status, string nick, uint pid, IPEndPoint remote, string reason)
        {
            string statusString = string.Format(@" [{0}] Nick:{1}-PID:{2}-IP:{3}-Reason:{4}", status, nick, pid, remote, reason);
            LogWriter.ToLog(Serilog.Events.LogEventLevel.Information, statusString);
        }

        public virtual string RequstFormatConversion(string message)
        {
            if (message.Contains("login"))
            {
                message = message.Replace(@"\-", @"\");
                message = message.Replace('-', '\\');

                int pos = message.IndexesOf("\\")[1];

                if (message.Substring(pos, 2) != "\\\\")
                {
                    message = message.Insert(pos, "\\");
                }
                return message;
            }
            else
            {
                return message;
            }
        }
    }
}