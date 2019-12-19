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

        /// <summary>
        /// Our CRC16 object for generating Checksums
        /// </summary>
        protected static Crc16 Crc = new Crc16(Crc16Mode.Standard);


        public GPCMPlayerInfo PlayerInfo { get; protected set; }
        public GPCMSession(TemplateTcpServer server) : base(server)
        {
            DisconnectAfterSend = false;
            PlayerInfo = new GPCMPlayerInfo();
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
                    continue;
                // Read client message, and parse it into key value pairs
                string[] recieved = command.TrimStart('\\').Split('\\');

                Dictionary<string, string> dict = GameSpyUtils.ConvertGPResponseToKeyValue(recieved);

                CommandSwitcher.Switch(this, dict);
            }
        }
        //when a user is loged in we update the sessionkey and the Guid to database
        protected override void OnConnected()
        {
            PlayerInfo.LoginProcess = LoginStatus.Connected;
            ToLog($"[Conn] ID:{Id} IP:{Server.Endpoint.Address.ToString()}");
            SendServerChallenge();
        }
        protected override void OnDisconnected()
        {
            PlayerInfo.LoginProcess = LoginStatus.Disconnected;
            ToLog($"[Disc] ID:{Id} IP:{Server.Endpoint.Address.ToString()}");
            RemoveGuidAndSessionKeyFromDatabase();
        }

        private void RemoveGuidAndSessionKeyFromDatabase()
        {
            GPCMServer.DB.Execute("UPDATE namespace SET guid = null WHERE guid = @P1", PlayerInfo.SessionKey, Id);
            GPCMServer.DB.Execute("UPDATE profiles SET status = @P0, statstring = '' WHERE profileid = @P1", (uint)PlayerStatus.Offline, PlayerInfo.Profileid);
            //whether need to check if there exsit guid and session key ?
            //set the status and status string to default
        }

        public void SendServerChallenge()
        {
            // Only send the login challenge once
            if (PlayerInfo.LoginProcess != LoginStatus.Connected)
            {
                DisconnectByReason(DisconnectReason.ClientChallengeAlreadySent);
                // Throw the error                
                ToLog(LogLevel.Warning, "The server challenge has already been sent. Cannot send another login challenge.");
            }

            // We send the client the challenge key
            string serverChallengeKey = GameSpyLib.Common.GameSpyRandom.GenerateRandomString(10, GameSpyLib.Common.GameSpyRandom.StringType.Alpha);
            PlayerInfo.ServerChallenge = serverChallengeKey;
            PlayerInfo.LoginProcess = LoginStatus.Processing;
            string sendingBuffer = string.Format(@"\lc\1\challenge\{0}\id\{1}\final\", serverChallengeKey, 1);
            SendAsync(sendingBuffer);
        }



        public void DisconnectByReason(DisconnectReason reason)
        {
            ToLog(reason.ToString());
            Disconnect();
        }


        public void StatusToLog(string status, string nick, uint pid, IPEndPoint remote, string reason)
        {
            string statusString = string.Format(@" [{0}] Nick:{1}-PID:{2}-IP:{3}-Reason:{4}", status, nick, pid, remote, reason);
            LogWriter.Log.Write(LogLevel.Info, statusString);
        }
    }
}
