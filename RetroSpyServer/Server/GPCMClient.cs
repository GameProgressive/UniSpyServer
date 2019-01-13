using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using GameSpyLib;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using GameSpyLib.Server;
using GameSpyLib.Database;

namespace RetroSpyServer.Server
{
    public enum LoginStatus
    {
        Connected,
        Processing,
        Completed,
        Disconnected
    }

    public enum PlayerStatus : int
    {
        /// <summary>
        /// The player is offline
        /// </summary>
        Offline,

        /// <summary>
        /// The player is online
        /// </summary>
        Online,

        /// <summary>
        /// The player is banned
        /// </summary>
        Banned
    }

    public enum DisconnectReason : int
    {
        /// <summary>
        /// Client sends the "logout" command
        /// </summary>
        NormalLogout,

        /// <summary>
        /// Keep Alive Packet failed to send (may not work with new async socket code)
        /// </summary>
        KeepAliveFailed,

        /// <summary>
        /// The client failed to complete the login 15 seconds after creating the connection
        /// </summary>
        LoginTimedOut,

        /// <summary>
        /// The username sent by the client does not exist
        /// </summary>
        InvalidUsername,

        /// <summary>
        /// The provided password for the player nick is incorrect
        /// </summary>
        InvalidPassword,

        /// <summary>
        /// Invalid login query sent by client
        /// </summary>
        InvalidLoginQuery,

        /// <summary>
        /// Create player failed, username exists already
        /// </summary>
        CreateFailedUsernameExists,

        /// <summary>
        /// Failed to create new player account due to a database exception
        /// </summary>
        CreateFailedDatabaseError,

        /// <summary>
        /// A general login failure (check error log)
        /// </summary>
        GeneralError,

        /// <summary>
        /// The stream disconnected unexpectedly. This can happen if the user clicks the
        /// "Quit" button on the top of the main menu instead of the "Logout" button.
        /// </summary>
        Disconnected,

        /// <summary>
        /// The player was forcefully logged out by console command
        /// </summary>
        ForcedLogout,

        /// <summary>
        /// A new login detected with the old player session still logged in
        /// </summary>
        NewLoginDetected,

        /// <summary>
        /// Forced server shutdown
        /// </summary>
        ForcedServerShutdown,

        /// <summary>
        /// The client challenge was already sent by the server for this connection
        /// </summary>
        ClientChallengeAlreadySent,

        /// <summary>
        /// The player is banned and cannot login
        /// </summary>
        PlayerIsBanned,

        /// <summary>
        /// The player information is not valid
        /// </summary>
        InvalidPlayer
    }

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
        /// Gets the current login status
        /// </summary>
        public LoginStatus Status { get; protected set; }

        /// <summary>
        /// Indicates whether this player successfully completed the login process
        /// </summary>
        public bool CompletedLoginProcess { get; protected set; } = false;

        /// <summary>
        /// The connected clients Player Id
        /// </summary>
        public uint PlayerId { get; protected set; }

        /// <summary>
        /// The connected clients Nick
        /// </summary>
        public string PlayerNick { get; protected set; }

        /// <summary>
        /// The connected clients Email Address
        /// </summary>
        public string PlayerEmail { get; protected set; }

        /// <summary>
        /// The connected clients country code
        /// </summary>
        public string PlayerCountryCode { get; protected set; }

        /// <summary>
        /// The clients password, MD5 hashed from UTF8 bytes
        /// </summary>
        private string PasswordHash;

        /// <summary>
        /// The TcpClient's Endpoint
        /// </summary>
        public IPEndPoint RemoteEndPoint { get; protected set; }

        /// <summary>
        /// The profile id parameter that is sent back to the client is initially 2, 
        /// and then 5 everytime after that. So we set here, whether we have sent the 
        /// profile to the client initially (with \id\2) yet.
        /// </summary>
        private bool ProfileSent = false;

        /// <summary>
        /// The users session key
        /// </summary>
        private ushort SessionKey;

        /// <summary>
        /// The Servers challange key, sent when the client first connects.
        /// This is used as part of the hash used to "proove" to the client
        /// that the password in our database matches what the user enters
        /// </summary>
        private string ServerChallengeKey;

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
        public TCPStream Stream { get; protected set; }

        /// <summary>
        /// The date time of when this connection was created. Used to disconnect user
        /// connections that hang
        /// </summary>
        private DateTime ConnectionCreated = DateTime.Now;

        /// <summary>
        /// Our CRC16 object for generating Checksums
        /// </summary>
        protected static Crc16 Crc = new Crc16(Crc16Mode.Standard);

        /// <summary>
        /// An Event that is fired when the client successfully logs in.
        /// </summary>
        public static event ConnectionUpdate OnSuccessfulLogin;

        /// <summary>
        /// Event fired when that remote connection logs out, or
        /// the socket gets disconnected. This event will not fire
        /// unless OnSuccessfulLogin event was fired first.
        /// </summary>
        public static event GpcmConnectionClosed OnDisconnect;

        private DatabaseDriver databaseDriver = null;

        #endregion Variables

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ReadArgs">The Tcp Client connection</param>
        public GPCMClient(TCPStream ConnectionStream, long ConnectionId, DatabaseDriver databaseDriver)
        {
            // Set default variable values
            PlayerNick = "Connecting...";
            PlayerId = 0;
            RemoteEndPoint = (IPEndPoint)ConnectionStream.RemoteEndPoint;
            Disposed = false;
            Status = LoginStatus.Connected;

            // Set the connection ID
            this.ConnectionId = ConnectionId;
            this.databaseDriver = databaseDriver;

            // Create our Client Stream
            Stream = ConnectionStream;
            Stream.OnDisconnect += OnStreamDisconnects;
            Stream.DataReceived += OnDataReceived;
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
                Stream.OnDisconnect -= OnStreamDisconnects;
                Stream.DataReceived -= OnDataReceived;
                Stream.Close(reason == DisconnectReason.ForcedServerShutdown);
            }
            catch { }

            // Set status and log
            if (Status == LoginStatus.Completed)
            {
                if (reason == DisconnectReason.NormalLogout)
                {
                    LogWriter.Log.Write(
                        "Client Logout:  {0} - {1} - {2}",
                        LogLevel.Information,
                        PlayerNick,
                        PlayerId,
                        RemoteEndPoint
                    );
                }
                else if (reason != DisconnectReason.ForcedServerShutdown)
                {
                    LogWriter.Log.Write(
                        "Client Disconnected:  {0} - {1} - {2}, Code={3}",
                        LogLevel.Information,
                        PlayerNick,
                        PlayerId,
                        RemoteEndPoint,
                        Enum.GetName(typeof(DisconnectReason), reason)
                    );
                }
            }

            // Preapare to be unloaded from memory
            Status = LoginStatus.Disconnected;
            Dispose();

            // Call disconnect event
            OnDisconnect?.Invoke(this);
        }

        #region Stream Callbacks

        /// <summary>
        /// Main listner loop. Keeps an open stream between the client and server while
        /// the client is logged in / playing
        /// </summary>
        private void OnDataReceived(TCPStream stream, string message)
        {
            if (stream != Stream)
                return;

            // Read client message, and parse it into key value pairs
            string[] recieved = message.TrimStart('\\').Split('\\');
            switch (recieved[0])
            {
                case "newuser":
                    CreateNewUser(PresenceServer.ConvertToKeyValue(recieved));
                    break;
                case "login":
                    ProcessLogin(PresenceServer.ConvertToKeyValue(recieved));
                    break;
                case "getprofile":
                    SendProfile();
                    break;
                case "updatepro":
                    UpdateUser(PresenceServer.ConvertToKeyValue(recieved));
                    break;
                case "logout":
                    Disconnect(DisconnectReason.NormalLogout);
                    break;
                default:
                    LogWriter.Log.Write("Received unknown request " + recieved[0], LogLevel.Debug);
                    GameSpyLib.Server.PresenceServer.SendError(stream, 0, "An invalid request was sended.");
                    stream.Close();
                    break;
            }
        }

        /// <summary>
        /// Event fired when the stream disconnects unexpectedly
        /// </summary>
        private void OnStreamDisconnects(TCPStream stream)
        {
            if (stream != Stream)
                return;

            Disconnect(DisconnectReason.Disconnected);
        }

        #endregion Stream Callbacks

        #region Login Steps

        /// <summary>
        ///  This method starts off by sending a random string 10 characters
        ///  in length, known as the Server challenge key. This is used by 
        ///  the client to return a client challenge key, which is used
        ///  to validate login information later.
        ///  </summary>
        public void SendServerChallenge(uint ServerID)
        {
            // Only send the login challenge once
            if (Status != LoginStatus.Connected)
            {
                // Create an exception message
                TimeSpan ts = DateTime.Now - Created;

                // Disconnect user
                Disconnect(DisconnectReason.ClientChallengeAlreadySent);

                // Throw the error
                throw new Exception("The server challenge has already been sent. Cannot send another login challenge." + $"\tChallenge was sent \"{ts.ToString()}\" ago.");
            }

            // We send the client the challenge key
            ServerChallengeKey = GameSpyLib.Random.GenerateRandomString(10, GameSpyLib.Random.StringType.Alpha);
            Status = LoginStatus.Processing;
            Stream.SendAsync(@"\lc\1\challenge\{0}\id\{1}\final\", ServerChallengeKey, ServerID);
        }

        /// <summary>
        /// This method verifies the login information sent by
        /// the client, and returns encrypted data for the client
        /// to verify as well
        /// </summary>
        public void ProcessLogin(Dictionary<string, string> Recv)
        {
            // Make sure we have all the required data to process this login
            if (!Recv.ContainsKey("uniquenick") || !Recv.ContainsKey("challenge") || !Recv.ContainsKey("response"))
            {
                GameSpyLib.Server.PresenceServer.SendError(Stream, 0, "Invalid response received from the client!");
                Disconnect(DisconnectReason.InvalidLoginQuery);
                return;
            }

            // Dispose connection after use
            try
            {
                // Try and fetch the user from the database
                Dictionary<string, object> User = DatabaseUtility.GetUser(databaseDriver, Recv["uniquenick"]);
                if (User == null)
                {
                    GameSpyLib.Server.PresenceServer.SendError(Stream, 265, "The uniquenick provided is incorrect!");
                    Disconnect(DisconnectReason.InvalidUsername);
                    return;
                }

                // Check if user is banned
                PlayerStatus currentPlayerStatus;

                if (!Enum.TryParse(User["status"].ToString(), out currentPlayerStatus))
                {
                    GameSpyLib.Server.PresenceServer.SendError(Stream, 265, "Invalid player data! Please contact an administrator.");
                    Disconnect(DisconnectReason.InvalidPlayer);
                    return;
                }

                if (currentPlayerStatus == PlayerStatus.Banned)
                {
                    GameSpyLib.Server.PresenceServer.SendError(Stream, 265, "Your accout has been permanently suspended.");
                    Disconnect(DisconnectReason.PlayerIsBanned);
                    return;
                }

                // Set player variables
                PlayerId = uint.Parse(User["profileid"].ToString());
                PlayerNick = Recv["uniquenick"];
                PlayerEmail = User["email"].ToString();
                PlayerCountryCode = User["countrycode"].ToString();
                PasswordHash = User["password"].ToString().ToLowerInvariant();

                // Use the GenerateProof method to compare with the "response" value. This validates the given password
                if (Recv["response"] == GenerateProof(Recv["challenge"], ServerChallengeKey))
                {
                    // Create session key
                    SessionKey = Crc.ComputeChecksum(PlayerNick);

                    // Password is correct
                    Stream.SendAsync(
                        @"\lc\2\sesskey\{0}\proof\{1}\userid\{2}\profileid\{2}\uniquenick\{3}\lt\{4}__\id\1\final\",
                        SessionKey,
                        GenerateProof(ServerChallengeKey, Recv["challenge"]), // Do this again, Params are reversed!
                        PlayerId,
                        PlayerNick,
                        GameSpyLib.Random.GenerateRandomString(24, GameSpyLib.Random.StringType.Hex) // Generate LT whatever that is (some sort of random string, 22 chars long)
                    );

                    // Log Incoming Connections
                    LogWriter.Log.Write("Client Login:   {0} - {1} - {2}", LogLevel.Information, PlayerNick, PlayerId, RemoteEndPoint);

                    // Update status last, and call success login
                    Status = LoginStatus.Completed;
                    CompletedLoginProcess = true;
                    OnSuccessfulLogin?.Invoke(this);
                }
                else
                {
                    // Log Incoming Connections
                    LogWriter.Log.Write("Failed Login Attempt: {0} - {1} - {2}", LogLevel.Information, PlayerNick, PlayerId, RemoteEndPoint);

                    // Password is incorrect with database value
                    Stream.SendAsync(@"\error\\err\260\fatal\\errmsg\The password provided is incorrect.\id\1\final\");
                    Disconnect(DisconnectReason.InvalidPassword);
                }
            }
            catch (Exception ex)
            {
                LogWriter.Log.Write(ex.ToString(), LogLevel.Error);
                Disconnect(DisconnectReason.GeneralError);
                return;
            }
        }

        /// <summary>
        /// This method is called when the client requests for the Account profile
        /// </summary>
        private void SendProfile()
        {
            Stream.SendAsync(
                @"\pi\\profileid\{0}\nick\{1}\userid\{0}\email\{2}\sig\{3}\uniquenick\{1}\pid\0\firstname\\lastname\" +
                @"\countrycode\{4}\birthday\16844722\lon\0.000000\lat\0.000000\loc\\id\{5}\\final\",
                PlayerId, PlayerNick, PlayerEmail, GameSpyLib.Random.GenerateRandomString(33, GameSpyLib.Random.StringType.Hex), PlayerCountryCode, (ProfileSent ? "5" : "2")
            );

            // Set that we send the profile initially
            if (!ProfileSent) ProfileSent = true;
        }

        #endregion Steps

        #region User Methods

        /// <summary>
        /// Whenever the "newuser" command is recieved, this method is called to
        /// add the new users information into the database
        /// </summary>
        /// <param name="Recv">Array of parms sent by the server</param>
        private void CreateNewUser(Dictionary<string, string> Recv)
        {
            // Make sure the user doesnt exist already
            try
            {
                // Check to see if user exists
                if (DatabaseUtility.UserExists(databaseDriver, Recv["nick"]))
                {
                    Stream.SendAsync(@"\error\\err\516\fatal\\errmsg\This account name is already in use!\id\1\final\");
                    Disconnect(DisconnectReason.CreateFailedUsernameExists);
                    return;
                }

                // We need to decode the Gamespy specific encoding for the password
                string Password = GamespyUtils.DecodePassword(Recv["passwordenc"]);
                string Cc = (RemoteEndPoint.AddressFamily == AddressFamily.InterNetwork)
                    //? GeoIP.GetCountryCode(RemoteEndPoint.Address)
                    //: "US";
                    ? "US" : "US";

                // Attempt to create account. If Pid is 0, then we couldnt create the account. TODO: Handle Unique Nickname
                if ((PlayerId = DatabaseUtility.CreateUser(databaseDriver, Recv["nick"], Password, Recv["email"], Cc, Recv["nick"])) == 0)
                {
                    PresenceServer.SendError(Stream, 516, "An error oncurred while creating the account!");
                    Disconnect(DisconnectReason.CreateFailedDatabaseError);
                    return;
                }

                Stream.SendAsync(@"\nur\\userid\{0}\profileid\{0}\id\1\final\", PlayerId);
            }
            catch (Exception e)
            {
                // Check for invalid query params
                if (e is KeyNotFoundException)
                {
                    PresenceServer.SendError(Stream, 516, "Invalid response received from the client!");
                }
                else
                {
                    PresenceServer.SendError(Stream, 516, "An error oncurred while creating the account!");
                    LogWriter.Log.Write("An error occured while trying to create a new User account :: " + e.Message, LogLevel.Error);
                }

                Disconnect(DisconnectReason.GeneralError);
                return;
            }
        }


        /// <summary>
        /// Updates the Users Country code when sent by the client
        /// </summary>
        /// <param name="recv">Array of information sent by the server</param>
        private void UpdateUser(Dictionary<string, string> Recv)
        {
            // Set clients country code
            try
            {
                DatabaseUtility.UpdateUser(databaseDriver, PlayerId, Recv["countrycode"]);
            }
            catch (Exception e)
            {
                LogWriter.Log.Write(e.Message, LogLevel.Error);
            }
        }

        /// <summary>
        /// Polls the connection, and checks for drops
        /// </summary>
        public void SendKeepAlive()
        {
            if (Status == LoginStatus.Completed)
            {
                // Try and send a Keep-Alive
                try
                {
                    Stream.SendAsync(@"\ka\\final\");
                }
                catch
                {
                    Disconnect(DisconnectReason.KeepAliveFailed);
                }
            }
        }

        #endregion

        #region Misc Methods

        /// <summary>
        /// Generates an MD5 hash, which is used to verify the clients login information
        /// </summary>
        /// <param name="challenge1">First challenge key</param>
        /// <param name="challenge2">Second challenge key</param>
        /// <returns>
        ///     The proof verification MD5 hash string that can be compared to what the client sends,
        ///     to verify that the users entered password matches the password in the database.
        /// </returns>
        private string GenerateProof(string challenge1, string challenge2)
        {
            // Generate our string to be hashed
            StringBuilder HashString = new StringBuilder(PasswordHash);
            HashString.Append(' ', 48); // 48 spaces
            HashString.Append(PlayerNick);
            HashString.Append(challenge1);
            HashString.Append(challenge2);
            HashString.Append(PasswordHash);
            return HashString.ToString().GetMD5Hash();
        }
        #endregion

        public bool Equals(GPCMClient other)
        {
            if (other == null) return false;
            return (PlayerId == other.PlayerId || PlayerNick == other.PlayerNick);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GPCMClient);
        }

        public override int GetHashCode()
        {
            return (int)PlayerId;
        }
    }
}
