using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using PresenceConnectionManager.Application;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler;
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
    public class GPCMClient : IDisposable, IEquatable<GPCMClient>
    {
        #region Variables

        /// <summary>
        /// Indicates whether this player successfully completed the login process
        /// </summary>
        public bool CompletedLoginProcess { get; set; } = false;

        /// <summary>
        /// The TcpClient's Endpoint
        /// </summary>
        public IPEndPoint RemoteEndPoint { get; protected set; }

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
        /// The users session key
        /// </summary>
        public ushort SessionKey { get; set; }


        /// <summary>
        /// The Servers challange key, sent when the client first connects.
        /// This is used as part of the hash used to "proove" to the client
        /// that the password in our database matches what the user enters
        /// </summary>
        public string ServerChallengeKey;

        /// <summary>
        /// Variable that determines if the client is disconnected,
        /// and this object can be cleared from memory
        /// </summary>
        public bool Disposed { get; protected set; }

        /// <summary>
        /// Indicates the connection ID for this connection
        /// </summary>
        public long ConnectionId { get; protected set; }

        /// <summary>
        /// Indicates the date and time this connection was created
        /// </summary>
        public readonly DateTime Created = DateTime.Now;

        /// <summary>
        /// The clients socket network stream
        /// </summary>
        public TcpStream Stream { get; protected set; }

        /// <summary>
        /// The date time of when this connection was created. Used to disconnect user
        /// connections that hang
        /// </summary>
        //private DateTime ConnectionCreated = DateTime.Now;

        /// <summary>
        /// Our CRC16 object for generating Checksums
        /// </summary>
        protected static Crc16 Crc = new Crc16(Crc16Mode.Standard);

        /// <summary>
        /// An Event that is fired when the client successfully logs in.
        /// </summary>
        public static event GPCMConnectionUpdate OnSuccessfulLogin;

        /// <summary>
        /// Event fired when that remote connection logs out, or
        /// the socket gets disconnected. This event will not fire
        /// unless OnSuccessfulLogin event was fired first.
        /// </summary>
        public static event GPCMConnectionClosed OnDisconnect;

        /// <summary>
        /// Event fired when the client status or location is changed,
        /// so the data could be notified to all clients
        /// </summary>
        public static event GPCMStatusChanged OnStatusChanged;

        public GPCMPlayerInfo PlayerInfo { get; protected set; }



        #endregion Variables

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ReadArgs">The Tcp Client connection</param>
        public GPCMClient(TcpStream stream, long connectionId)
        {

            PlayerInfo = new GPCMPlayerInfo();

            RemoteEndPoint = (IPEndPoint)stream.RemoteEndPoint;
            Disposed = false;

            // Set the connection ID
            ConnectionId = connectionId;

            SessionKey = 0;

            // Create our Client Stream
            Stream = stream;
            Stream.OnDisconnected += ClientDisconnected;
            Stream.OnDataReceived += ProcessData;
            Stream.IsMessageFinished += IsMessageFinished;
            Stream.BeginReceive();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~GPCMClient()
        {
            if (!Disposed)
                Dispose();
        }

        /// <summary>
        /// Disposes of the client object. The connection is no longer
        /// closed here and the Disconnect even is NO LONGER fired
        /// </summary>
        public void Dispose()
        {
            // Preapare to be unloaded from memory
            Disposed = true;
        }

        public void SendServerChallenge(uint ServerID)
        {
            // Only send the login challenge once
            if (PlayerInfo.LoginStatus != LoginStatus.Connected)
            {
                Disconnect(DisconnectReason.ClientChallengeAlreadySent);

                // Throw the error                
                LogWriter.Log.Write("The server challenge has already been sent. Cannot send another login challenge.", LogLevel.Warning);

            }

            // We send the client the challenge key
            ServerChallengeKey = GameSpyLib.Common.Random.GenerateRandomString(10, GameSpyLib.Common.Random.StringType.Alpha);
            PlayerInfo.LoginStatus = LoginStatus.Processing;
            Stream.SendAsync(@"\lc\1\challenge\{0}\id\{1}\final\", ServerChallengeKey, ServerID);
        }

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
        public void Disconnect(DisconnectReason reason)
        {
            // If connection is still alive, disconnect user
            try
            {

                Stream.OnDataReceived -= ProcessData;

                Stream.IsMessageFinished -= IsMessageFinished;

                Stream.OnDisconnected -= ClientDisconnected;

                Stream.Close(reason == DisconnectReason.ForcedServerShutdown);
            }
            catch { }

            // Set status and log
            if (PlayerInfo.LoginStatus == LoginStatus.Completed)
            {
                if (reason == DisconnectReason.NormalLogout)
                {
                    Stream.ToLog(LogLevel.Info, "Logout", "", "{0} - {1} - {2}", PlayerInfo.PlayerNick, PlayerInfo.PlayerId, RemoteEndPoint);
                    //LogWriter.Log.Write(
                    //    LogLevel.Info,
                    //    "{0,-8} [Logout] {1} - {2} - {3}",
                    //    Stream.ServerName,
                    //);
                }
                else if (reason != DisconnectReason.ForcedServerShutdown)
                {
                    Stream.ToLog(
                        LogLevel.Info,
                        "Disconnected", "",
                        "{0} - {1} - {2}, Code={3}",
                        PlayerInfo.PlayerNick,
                        PlayerInfo.PlayerId,
                        RemoteEndPoint,
                        Enum.GetName(typeof(DisconnectReason), reason));

                    //LogWriter.Log.Write(
                    //    LogLevel.Info,
                    //    "{0,-8} [Disconnected] {1} - {2} - {3}, Code={4}",
                    //    Stream.ServerName,

                    //);
                }
            }

            // Preapare to be unloaded from memory
            PlayerInfo.PlayerStatus = PlayerStatus.Offline;
            PlayerInfo.LoginStatus = LoginStatus.Disconnected;
            Dispose();

            // Call disconnect event
            OnDisconnect?.Invoke(this);
        }

        #region Stream Callbacks

        private bool IsMessageFinished(string message)
        {
            if (message.EndsWith("\\final\\"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Main listner loop. Keeps an open stream between the client and server while
        /// the client is logged in / playing
        /// </summary>
        private void ProcessData(string message)
        {
            if (message[0] != '\\')
            {
                GameSpyUtils.SendGPError(Stream, GPErrorCode.General, "An invalid request was sended.");
                return;
            }
            
            string[] submessage = message.Split("\\final\\");

            foreach (string command in submessage)
            {
                if (command.Length < 1)
                    continue;

                // Read client message, and parse it into key value pairs
                string[] recieved = command.TrimStart('\\').Split('\\');
                Dictionary<string, string> dict = GameSpyUtils.ConvertGPResponseToKeyValue(recieved);
                switch (recieved[0])
                {
                    case "inviteto":
                        InviteToHandler.AddProducts(this, dict);
                        break;
                    case "login":
                        LoginHandler.ProcessLogin(this, dict, OnSuccessfulLogin, OnStatusChanged);
                        break;
                    case "getprofile":
                        GetProfileHandler.SendProfile(this, dict);
                        break;
                    case "addbuddy":
                        AddBuddyHandler.Addfriends(this, dict);
                        break;
                    case "delbuddy":
                        DelBuddyHandler.Handle(this,dict);
                        break;
                    case "updateui":
                        UpdateUiHandler.UpdateUi(this, dict);
                        break;
                    case "updatepro":
                        UpdateProHandler.UpdateUser(this, dict);
                        break;
                    case "logout":
                        Disconnect(DisconnectReason.NormalLogout);
                        break;
                    case "status":
                        StatusHandler.UpdateStatus(this, dict, OnStatusChanged);
                        break;
                    case "newuser":
                        NewUserHandler.NewUser(this,dict);
                        break;
                    case "ka":
                        KAHandler.SendKeepAlive(this);
                        break;
                    default:
                        LogWriter.Log.Write("[GPCM] received unknown data " + recieved[0], LogLevel.Debug);
                        GameSpyUtils.SendGPError(Stream, GPErrorCode.General, "An invalid request was sended.");
                        break;
                }
            }
        }

        /// <summary>
        /// Event fired when the stream disconnects unexpectedly
        /// </summary>
        private void ClientDisconnected()
        {
            Disconnect(DisconnectReason.Disconnected);
        }

        #endregion Stream Callbacks

        #region Login Steps





        #endregion Steps






        public bool Equals(GPCMClient other)
        {
            if (other == null) return false;
            return (PlayerInfo.PlayerId == other.PlayerInfo.PlayerId || PlayerInfo.PlayerNick == other.PlayerInfo.PlayerNick);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GPCMClient);
        }

        public override int GetHashCode()
        {
            return (int)PlayerInfo.PlayerId;
        }

    }
}
