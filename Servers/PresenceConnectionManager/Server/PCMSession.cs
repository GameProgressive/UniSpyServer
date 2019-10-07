using System;
using System.Collections.Generic;
using System.Text;
using GameSpyLib.Delegate;
using GameSpyLib.Extensions;
using NetCoreServer;
using PresenceConnectionManager.Structures;

namespace PresenceConnectionManager.Server
{
    public class PCMSession : TcpSession
    {
        public event MessageFinished IsMessageFinished;
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

        ///// <summary>
        ///// An Event that is fired when the client successfully logs in.
        ///// </summary>
        //public static event GPCMConnectionUpdate OnSuccessfulLogin;

        ///// <summary>
        ///// Event fired when that remote connection logs out, or
        ///// the socket gets disconnected. This event will not fire
        ///// unless OnSuccessfulLogin event was fired first.
        ///// </summary>
        //public static event GPCMConnectionClosed OnDisconnect;

        ///// <summary>
        ///// Event fired when the client status or location is changed,
        ///// so the data could be notified to all clients
        ///// </summary>
        //public static event GPCMStatusChanged OnStatusChanged;

        public GPCMPlayerInfo PlayerInfo { get; protected set; }
        public PCMSession(TcpServer server) : base(server)
        {
            IsMessageFinished += CheckIsMessageFinished;
        }
        protected bool CheckIsMessageFinished(string message)
        {
            if (message.EndsWith("\\final\\"))
                return true;
            else
                return false;
        }
        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            bool closeclient = IsMessageFinished.Invoke(message);

        }
    }
}
