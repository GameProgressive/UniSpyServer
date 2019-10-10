using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Structures;
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
        /// The profile id parameter that is sent back to the client is initially 2, 
        /// and then 5 everytime after that. So we set here, whether we have sent the 
        /// profile to the client initially (with \id\2) yet.
        /// </summary>
        public bool ProfileSent = false;

        /// <summary>
        /// This boolean checks if the client has received buddy information
        /// </summary>
        public bool BuddiesSent = false;

        /// <summary>
        /// Indicates the date and time this connection was created
        /// </summary>
        public readonly DateTime Created = DateTime.Now;

        /// <summary>
        /// Our CRC16 object for generating Checksums
        /// </summary>
        protected static Crc16 Crc = new Crc16(Crc16Mode.Standard);

        public GPCMSession(TemplateTcpServer server) : base(server)
        {
            DisconnectAfterSend = false;
        }

        public GPCMPlayerInfo PlayerInfo { get; protected set; }
        public ushort SessionKey { get; internal set; }
        public string ServerChallengeKey { get; internal set; }

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
            ToLog($"[Conn] ID:{Id} IP:{Server.Endpoint.Address.ToString()}");
            SendServerChallenge();
        }
        protected override void OnDisconnected()
        {
            ToLog($"[Disc] ID:{Id} IP:{Server.Endpoint.Address.ToString()}");
            RemoveGuidAndSessionKeyFromDatabase();
        }

        private void RemoveGuidAndSessionKeyFromDatabase()
        {
            //whether need to check if there exsit guid and session key ?
            //set the status and status string to default
            throw new NotImplementedException();
        }

        public void SendServerChallenge()
        {
            // Only send the login challenge once
            //if (PlayerInfo.LoginStatus != LoginStatus.Connected)
            //{
            //    DisconnectByReason(DisconnectReason.ClientChallengeAlreadySent);

            //    // Throw the error                
            //    LogWriter.Log.Write("The server challenge has already been sent. Cannot send another login challenge.", LogLevel.Warning);
            //}
            // We send the client the challenge key
            string serverChallengeKey = GameSpyLib.Common.Random.GenerateRandomString(10, GameSpyLib.Common.Random.StringType.Alpha);
            PlayerInfo.LoginStatus = LoginStatus.Processing;
            string sendingBuffer = string.Format(@"\lc\1\challenge\{0}\id\{1}\final\", serverChallengeKey, 1);
            SendAsync(sendingBuffer);
        }

        public void DisconnectByReason(DisconnectReason reason)
        {
            ToLog(reason.ToString());
            Disconnect();
        }





        /// <summary>
        /// Event fired when the stream disconnects unexpectedly
        /// </summary>
        //protected override void ClientDisconnected()
        //{
        //    DisconnectByReason(DisconnectReason.Disconnected);
        //}

        /// <summary>
        /// Logs the client out of the game client, and closes the stream
        /// </summary>
        /// <param name="reason">
        /// The disconnect reason code. 
        /// </param>
        /// <remarks>
        /// If set the <paramref name="reason"/> is set to <see cref="DisconnectReason.ForcedServerShutdown"/>, 
        /// the OnDisconect event will still be called, but the EventArgs objects will NOT be returned to the IO pool. 
        /// You should only set to <see cref="DisconnectReason.ForcedServerShutdown"/> for a planned server shutdown.
        /// </remarks>
        //public void DisconnectByReason(DisconnectReason reason)
        //{
        //    // Set status and log
        //    if (PlayerInfo.LoginStatus == LoginStatus.Completed)
        //    {
        //        if (reason == DisconnectReason.NormalLogout)
        //        {
        //            StatusToLog("Logout", PlayerInfo.PlayerNick, PlayerInfo.PlayerId, RemoteEndPoint, Enum.GetName(typeof(DisconnectReason), reason));
        //        }
        //        else if (reason != DisconnectReason.ForcedServerShutdown)
        //        {
        //            StatusToLog("Disconnected", PlayerInfo.PlayerNick,
        //                PlayerInfo.PlayerId,
        //                ipa,
        //                Enum.GetName(typeof(DisconnectReason), reason));
        //        }
        //    }

        // Preapare to be unloaded from memory
        // PlayerInfo.PlayerStatus = PlayerStatus.Offline;
        //PlayerInfo.LoginStatus = LoginStatus.Disconnected;

        //  Dispose();

        // Call disconnect event
        // OnDisconnect?.Invoke(this);
        //}

        //public bool Equals(GPCMClient other)
        //{
        //    if (other == null) return false;
        //    return (PlayerInfo.PlayerId == other.PlayerInfo.PlayerId || PlayerInfo.PlayerNick == other.PlayerInfo.PlayerNick);
        //}

        public void StatusToLog(string status, string nick, uint pid, IPEndPoint remote, string reason)
        {
            string statusString = string.Format(@" [{0}] Nick:{1}-PID:{2}-IP:{3}-Reason:{4}", status, nick, pid, remote, reason);
            LogWriter.Log.Write(LogLevel.Info, statusString);
        }


        //public override bool Equals(object obj)
        //{
        //    return Equals(obj as GPCMClient);
        //}

        //public override int GetHashCode()
        //{
        //    return (int)PlayerInfo.PlayerId;
        //}

    }
}
