using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using GameSpyLib;
using GameSpyLib.Log;
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

    public enum UserStatus : int
    {
        /// <summary>
        /// The user is created, but not verified
        /// </summary>
        Created,
        /// <summary>
        /// The user is verified, and can login
        /// </summary>
        Verified,
        /// <summary>
        /// The user id banned
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
        InvalidPlayer,

        /// <summary>
        /// The player account is not activated
        /// </summary>
        PlayerIsNotActivated
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
        public LoginStatus LoginStatus { get; protected set; }

        /// <summary>
        /// Gets the current status of the player
        /// </summary>
        public PlayerStatus PlayerStatus { get; protected set; }

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
        /// The connected clients Authentication Token
        /// </summary>
        public string PlayerAuthToken { get; protected set; }

        /// <summary>
        /// The connected clients Unique Nick
        /// </summary>
        public string PlayerUniqueNick { get; protected set; }

        /// <summary>
        /// The connected clients country code
        /// </summary>
        public string PlayerCountryCode { get; protected set; }

        /// <summary>
        /// The clients password, MD5 hashed from UTF8 bytes
        /// </summary>
        private string PasswordHash;

        /// <summary>
        /// The clients status
        /// </summary>
        public string PlayerStatusString { get; protected set; }

        /// <summary>
        /// The place where the client is currently
        /// </summary>
        public string PlayerStatusLocation { get; protected set; }

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
        /// This boolean checks if the client has received buddy information
        /// </summary>
        private bool BuddiesSent = false;

        /// <summary>
        /// The users session key
        /// </summary>
        private ushort SessionKey = 0;

        public string PlayerFirstName { get; protected set; }
        public string PlayerLastName { get; protected set; }
        public int PlayerICQ { get; protected set; }
        public string PlayerHomepage { get; protected set; }
        public string PlayerZIPCode { get; protected set; }
        public string PlayerLocation { get; protected set; }
        public string PlayerAim { get; protected set; }
        public int PlayerOccupation { get; protected set; }
        public int PlayerIndustryID { get; protected set; }
        public int PlayerIncomeID { get; protected set; }
        public int PlayerMarried { get; protected set; }
        public int PlayerChildCount { get; protected set; }
        public int PlayerConnectionType { get; protected set; }
        public int PlayerPicture { get; protected set; }
        public int PlayerInterests { get; protected set; }
        public uint PlayerPublicMask { get; protected set; }
        public int PlayerOwnership { get; protected set; }
        public ushort PlayerBirthday { get; protected set; }
        public ushort PlayerBirthmonth { get; protected set; }
        public ushort PlayerBirthyear { get; protected set; }
        public PlayerSexType PlayerSex { get; protected set; }
        public float PlayerLatitude { get; protected set; }
        public float PlayerLongitude { get; protected set; }

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

        /// <summary>
        /// Event fired when the client status or location is changed,
        /// so the data could be notified to all clients
        /// </summary>
        public static event GpcmStatusChanged OnStatusChanged;

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
            PlayerStatusString = "Offline";
            PlayerLocation = "";
            PlayerId = 0;
            RemoteEndPoint = (IPEndPoint)ConnectionStream.RemoteEndPoint;
            Disposed = false;
            LoginStatus = LoginStatus.Connected;
            PlayerStatus = PlayerStatus.Offline;

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
            if (LoginStatus == LoginStatus.Completed)
            {
                if (reason == DisconnectReason.NormalLogout)
                {
                    LogWriter.Log.Write(
                        "Client Logout:  {0} - {1} - {2}",
                        LogLevel.Info,
                        PlayerNick,
                        PlayerId,
                        RemoteEndPoint
                    );
                }
                else if (reason != DisconnectReason.ForcedServerShutdown)
                {
                    LogWriter.Log.Write(
                        "Client Disconnected:  {0} - {1} - {2}, Code={3}",
                        LogLevel.Info,
                        PlayerNick,
                        PlayerId,
                        RemoteEndPoint,
                        Enum.GetName(typeof(DisconnectReason), reason)
                    );
                }
            }

            // Preapare to be unloaded from memory
            PlayerStatus = PlayerStatus.Offline;
            LoginStatus = LoginStatus.Disconnected;
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
                case "inviteto":
                    AddProducts(PresenceServer.ConvertToKeyValue(recieved));
                    break;
                case "newuser":
                    CreateNewUser(PresenceServer.ConvertToKeyValue(recieved));
                    break;
                case "login":
                    ProcessLogin(PresenceServer.ConvertToKeyValue(recieved));
                    break;
                case "getprofile":
                    SendProfile(PresenceServer.ConvertToKeyValue(recieved));
                    break;
                case "updatepro":
                    UpdateUser(PresenceServer.ConvertToKeyValue(recieved));
                    break;
                case "logout":
                    Disconnect(DisconnectReason.NormalLogout);
                    break;
                case "status":
                    UpdateStatus(PresenceServer.ConvertToKeyValue(recieved));
                    break;
                case "ka":
                    SendKeepAlive();
                    break;
                default:
                    LogWriter.Log.Write("Received unknown request " + recieved[0], LogLevel.Debug);
                    PresenceServer.SendError(stream, 0, "An invalid request was sended.");
                    stream.Close();
                    break;
            }
        }

        private void AddProducts(Dictionary<string, string> dictionary)
        {
            ushort readedSessionKey = 0;

            if (!dictionary.ContainsKey("products") || !dictionary.ContainsKey("sesskey"))
                return;

            if (!ushort.TryParse(dictionary["sesskey"], out readedSessionKey))
                return;

            if (readedSessionKey != SessionKey || readedSessionKey == 0)
                return;
                       
        }

        private void UpdateStatus(Dictionary<string, string> dictionary)
        {
            ushort testSK = 0;

            if (!dictionary.ContainsKey("statstring") || !dictionary.ContainsKey("locstring") || !dictionary.ContainsKey("sesskey"))
                return;

            if (!ushort.TryParse(dictionary["sesskey"], out testSK))
                return; // Invalid session key

            if (testSK != SessionKey)
                return; // Are you trying to update another user?

            PlayerStatusString = dictionary["statstring"];
            PlayerLocation = dictionary["locstring"];

            OnStatusChanged?.Invoke(this);
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
            if (LoginStatus != LoginStatus.Connected)
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
            LoginStatus = LoginStatus.Processing;
            Stream.SendAsync(@"\lc\1\challenge\{0}\id\{1}\final\", ServerChallengeKey, ServerID);
        }

        /// <summary>
        /// This method verifies the login information sent by
        /// the client, and returns encrypted data for the client
        /// to verify as well
        /// </summary>
        public void ProcessLogin(Dictionary<string, string> Recv)
        {
            uint partnerID = 0;

            // Make sure we have all the required data to process this login
            if (!Recv.ContainsKey("challenge") || !Recv.ContainsKey("response"))
            {
                PresenceServer.SendError(Stream, 0, "Invalid response received from the client!");
                Disconnect(DisconnectReason.InvalidLoginQuery);
                return;
            }

            // Parse the partnerid, required since it changes the challenge for Unique nick and User login
            if (Recv.ContainsKey("partnerid"))
            {
                if (!uint.TryParse(Recv["partnerid"], out partnerID))
                    partnerID = 0;
            }

            // Parse the 3 login types information
            if (Recv.ContainsKey("uniquenick"))
            {
                PlayerUniqueNick = Recv["uniquenick"];
            }
            else if (Recv.ContainsKey("authtoken"))
            {
                PlayerAuthToken = Recv["authtoken"];
            }
            else if (Recv.ContainsKey("user"))
            {
                // "User" is <nickname>@<email>
                string User = Recv["user"];
                int Pos = User.IndexOf('@');
                PlayerNick = User.Substring(0, Pos);
                PlayerEmail = User.Substring(Pos + 1);
            }

            // Dispose connection after use
            try
            {
                // Try and fetch the user from the database
                Dictionary<string, object> QueryResult;

                try
                {
                    if (PlayerUniqueNick != null)
                        QueryResult = DatabaseUtility.GetUserFromUniqueNick(databaseDriver, Recv["uniquenick"]);
                    else if (PlayerAuthToken != null)
                    {
                        //TODO! Add the database entry
                        PresenceServer.SendError(Stream, 0, "AuthToken is not supported yet");
                        return;
                    }
                    else
                        QueryResult = DatabaseUtility.GetUserFromNickname(databaseDriver, PlayerEmail, PlayerNick);
                }
                catch (Exception)
                {
                    PresenceServer.SendError(Stream, 4, "This request cannot be processed because of a database error.");
                    return;
                }

                if (QueryResult == null)
                {
                    if (PlayerUniqueNick != null)
                        PresenceServer.SendError(Stream, 265, "The unique nickname provided is incorrect!");
                    else
                        PresenceServer.SendError(Stream, 265, "The nickname provided is incorrect!");

                    Disconnect(DisconnectReason.InvalidUsername);
                    return;
                }

                // Check if user is banned
                PlayerStatus currentPlayerStatus;
                UserStatus currentUserStatus;

                if (!Enum.TryParse(QueryResult["status"].ToString(), out currentPlayerStatus))
                {
                    PresenceServer.SendError(Stream, 265, "Invalid player data! Please contact an administrator.");
                    Disconnect(DisconnectReason.InvalidPlayer);
                    return;
                }

                if (!Enum.TryParse(QueryResult["userstatus"].ToString(), out currentUserStatus))
                {
                    PresenceServer.SendError(Stream, 265, "Invalid player data! Please contact an administrator.");
                    Disconnect(DisconnectReason.InvalidPlayer);
                    return;
                }

                // Check the status of the account.
                // If the single profile is banned, the account or the player status

                if (currentPlayerStatus == PlayerStatus.Banned)
                {
                    PresenceServer.SendError(Stream, 265, "Your profile has been permanently suspended.");
                    Disconnect(DisconnectReason.PlayerIsBanned);
                    return;
                }

                if (currentUserStatus == UserStatus.Created)
                {
                    PresenceServer.SendError(Stream, 265, "Your account is not verified. Please check your email inbox and verify the account.");
                    Disconnect(DisconnectReason.PlayerIsBanned);
                    return;
                }

                if (currentUserStatus == UserStatus.Banned)
                {
                    PresenceServer.SendError(Stream, 265, "Your account has been permanently suspended.");
                    Disconnect(DisconnectReason.PlayerIsBanned);
                    return;
                }

                // Set player variables
                PlayerId = uint.Parse(QueryResult["profileid"].ToString());
                PasswordHash = QueryResult["password"].ToString().ToLowerInvariant();
                PlayerCountryCode = QueryResult["countrycode"].ToString();

                PlayerFirstName = QueryResult["firstname"].ToString();
                PlayerLastName = QueryResult["lastname"].ToString();
                PlayerICQ = int.Parse(QueryResult["icq"].ToString());
                PlayerHomepage = QueryResult["homepage"].ToString();
                PlayerZIPCode = QueryResult["zipcode"].ToString();
                PlayerLocation = QueryResult["location"].ToString();
                PlayerAim = QueryResult["aim"].ToString();
                PlayerOwnership = int.Parse(QueryResult["ownership1"].ToString());
                PlayerOccupation = int.Parse(QueryResult["occupationid"].ToString());
                PlayerIndustryID = int.Parse(QueryResult["industryid"].ToString());
                PlayerIncomeID = int.Parse(QueryResult["incomeid"].ToString());
                PlayerMarried = int.Parse(QueryResult["marriedid"].ToString());
                PlayerChildCount = int.Parse(QueryResult["childcount"].ToString());
                PlayerConnectionType = int.Parse(QueryResult["connectiontype"].ToString());
                PlayerPicture = int.Parse(QueryResult["picture"].ToString());
                PlayerInterests = int.Parse(QueryResult["interests1"].ToString());
                PlayerBirthday = ushort.Parse(QueryResult["birthday"].ToString());
                PlayerBirthmonth = ushort.Parse(QueryResult["birthmonth"].ToString());
                PlayerBirthyear = ushort.Parse(QueryResult["birthyear"].ToString());

                PlayerSexType playerSexType;
                if (!Enum.TryParse(QueryResult["sex"].ToString().ToUpper(), out playerSexType))
                    PlayerSex = PlayerSexType.PAT;
                else
                    PlayerSex = playerSexType;

                PlayerLatitude = float.Parse(QueryResult["latitude"].ToString());
                PlayerLongitude = float.Parse(QueryResult["longitude"].ToString());
                PlayerPublicMask = uint.Parse(QueryResult["publicmask"].ToString());

                string challengeData = "";

                if (PlayerUniqueNick != null)
                {
                    PlayerEmail = QueryResult["email"].ToString();
                    PlayerNick = QueryResult["nick"].ToString();
                    challengeData = PlayerUniqueNick;
                }
                else if (PlayerAuthToken != null)
                {
                    PlayerEmail = QueryResult["email"].ToString();
                    PlayerNick = QueryResult["nick"].ToString();
                    PlayerUniqueNick = QueryResult["uniquenick"].ToString();
                    challengeData = PlayerAuthToken;
                }
                else
                {
                    PlayerUniqueNick = QueryResult["uniquenick"].ToString();
                    challengeData = Recv["user"];
                }

                // Use the GenerateProof method to compare with the "response" value. This validates the given password
                if (Recv["response"] == GenerateProof(Recv["challenge"], ServerChallengeKey, challengeData, PlayerAuthToken != null ? 0 : partnerID))
                {
                    // Create session key
                    SessionKey = Crc.ComputeChecksum(PlayerUniqueNick);

                    // Password is correct
                    Stream.SendAsync(
                        @"\lc\2\sesskey\{0}\proof\{1}\userid\{2}\profileid\{2}\uniquenick\{3}\lt\{4}__\id\1\final\",
                        SessionKey,
                        GenerateProof(ServerChallengeKey, Recv["challenge"], challengeData, PlayerAuthToken != null ? 0 : partnerID), // Do this again, Params are reversed!
                        PlayerId,
                        PlayerNick,
                        GameSpyLib.Random.GenerateRandomString(22, GameSpyLib.Random.StringType.Hex) // Generate LT whatever that is (some sort of random string, 22 chars long)
                    );

                    // Log Incoming Connections
                    LogWriter.Log.Write("Client Login:   {0} - {1} - {2}", LogLevel.Info, PlayerNick, PlayerId, RemoteEndPoint);

                    // Update status last, and call success login
                    LoginStatus = LoginStatus.Completed;
                    PlayerStatus = PlayerStatus.Online;
                    PlayerStatusString = "Online";
                    PlayerStatusLocation = "";

                    CompletedLoginProcess = true;
                    OnSuccessfulLogin?.Invoke(this);
                    OnStatusChanged?.Invoke(this);

                    SendBuddies();
                }
                else
                {
                    // Log Incoming Connections
                    LogWriter.Log.Write("Failed Login Attempt: {0} - {1} - {2}", LogLevel.Info, PlayerNick, PlayerId, RemoteEndPoint);

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
        /// This method is called when the server needs to send the buddies to the client
        /// </summary>
        private void SendBuddies()
        {
            if (BuddiesSent)
                return;

            /*Stream.SendAsync(
                @"\bdy\1\list\2,\final\");

            Stream.SendAsync(
            //    @"\bm\100\f\2\msg\|s|0|ss|Offline\final\"
            @"\bm\100\f\2\msg\Messaggio di prova|s|2|ss|Home|ls|locstr://Reversing the world...|\final\"
            );*/

            Stream.SendAsync(@"\bdy\0\list\\final\");

            BuddiesSent = true;
        }

        /// <summary>
        /// This method is called when the client requests for the Account profile
        /// </summary>
        private void SendProfile(Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey("profileid"))
            {
                PresenceServer.SendError(Stream, 1, "There was an error parsing an incoming request.");
                return;
            }

            uint targetPID = 0, messID = 0;
            if (!uint.TryParse(dict["profileid"], out targetPID))
            {
                PresenceServer.SendError(Stream, 1, "There was an error parsing an incoming request.");
                return;
            }

            if (!uint.TryParse(dict["id"], out messID))
            {
                PresenceServer.SendError(Stream, 1, "There was an error parsing an incoming request.");
                return;
            }

            string datatoSend = @"\pi\\profileid\" + targetPID + @"\mp\4";

            // If the client want to access the public information
            // of another client
            if (targetPID != PlayerId)
            {
                uint publicMask = 0;

                var Query = DatabaseUtility.GetProfileInfo(databaseDriver, targetPID);
                if (Query == null)
                {
                    PresenceServer.SendError(Stream, 4, "Unable to get profile information.");
                    return;
                }

                if (!uint.TryParse(Query["publicmask"].ToString(), out publicMask))
                    publicMask = (uint)PublicMasks.MASK_NONE;

                datatoSend = string.Format(datatoSend + @"\nick\{0}\uniquenick\{1}\id\{2}", Query["nick"].ToString(), Query["uniquenick"].ToString(), messID);

                if (Query["email"].ToString().Length > 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                {
                    if ((publicMask & (uint)PublicMasks.MASK_EMAIL) > 0)
                        datatoSend += @"\email\" + Query["email"].ToString();
                }

                if (Query["lastname"].ToString().Length > 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\lastname\" + Query["lastname"].ToString();

                if (Query["firstname"].ToString().Length > 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\firstname\" + Query["firstname"].ToString();

                if (int.Parse(Query["icq"].ToString()) != 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\icquin\" + int.Parse(Query["icq"].ToString());

                if (PlayerHomepage.Length > 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                {
                    if ((publicMask & (uint)PublicMasks.MASK_HOMEPAGE) > 0)
                        datatoSend += @"\homepage\" + Query["homepage"].ToString();
                }

                if (uint.Parse(Query["picture"].ToString()) != 0)
                    datatoSend += @"\pic\" + uint.Parse(Query["Show"].ToString());

                if (Query["aim"].ToString().Length > 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\aim\" + Query["aim"].ToString();

                if (int.Parse(Query["occupationid"].ToString()) != 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\occ\" + int.Parse(Query["occupationid"].ToString());

                if (Query["zipcode"].ToString().Length > 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                {
                    if ((publicMask & (uint)PublicMasks.MASK_ZIPCODE) > 0)
                        datatoSend += @"\zipcode\" + Query["zipcode"].ToString();
                }

                if (Query["countrycode"].ToString().Length > 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                {
                    if ((publicMask & (uint)PublicMasks.MASK_COUNTRYCODE) > 0)
                        datatoSend += @"\countrycode\" + Query["countrycode"].ToString();
                }

                if (ushort.Parse(Query["birthday"].ToString()) > 0 && ushort.Parse(Query["birthmonth"].ToString()) > 0 && ushort.Parse(Query["birthyear"].ToString()) > 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                {
                    if ((publicMask & (uint)PublicMasks.MASK_BIRTHDAY) > 0)
                        datatoSend += @"\birthday\" + (uint)((ushort.Parse(Query["birthday"].ToString()) << 24) | (ushort.Parse(Query["birthmonth"].ToString()) << 16) | ushort.Parse(Query["birthyear"].ToString()));
                }

                if (Query["location"].ToString().Length > 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\loc\" + Query["location"].ToString();

                if (publicMask != (uint)PublicMasks.MASK_NONE && (publicMask & (uint)PublicMasks.MASK_SEX) > 0)
                {
                    PlayerSexType sexType;
                    if (Enum.TryParse(Query["sex"].ToString(), out sexType))
                    {
                        if (PlayerSex == PlayerSexType.FEMALE)
                            datatoSend += @"\sex\1";
                        else if (PlayerSex == PlayerSexType.MALE)
                            datatoSend += @"\sex\0";
                    }
                }

                if (float.Parse(Query["latitude"].ToString()) != 0.0f && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\lat\" + float.Parse(Query["latitude"].ToString());

                if (float.Parse(Query["longitude"].ToString()) != 0.0f && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\lon\" + float.Parse(Query["longitude"].ToString());

                if (int.Parse(Query["incomeid"].ToString()) != 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\inc\" + int.Parse(Query["incomeid"].ToString());

                if (int.Parse(Query["industryid"].ToString()) != 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\ind\" + int.Parse(Query["industryid"].ToString());

                if (int.Parse(Query["marriedid"].ToString()) != 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\mar\" + int.Parse(Query["marriedid"].ToString());

                if (int.Parse(Query["childcount"].ToString()) != 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\chc\" + int.Parse(Query["childcount"].ToString());

                if (int.Parse(Query["interests1"].ToString()) != 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\i1\" + int.Parse(Query["interests1"].ToString());

                if (int.Parse(Query["ownership1"].ToString()) != 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\o1\" + int.Parse(Query["ownership1"].ToString());

                if (int.Parse(Query["connectiontype"].ToString()) != 0 && publicMask != (uint)PublicMasks.MASK_NONE)
                    datatoSend += @"\conn\" + int.Parse(Query["connectiontype"].ToString());

                // SUPER NOTE: Please check the Signature of the PID, otherwise when it will be compared with other peers, it will break everything (See gpiPeer.c @ peerSig)
                datatoSend += @"\sig\" + GameSpyLib.Random.GenerateRandomString(33, GameSpyLib.Random.StringType.Hex) + @"\final\";
            }
            else
            {
                // Since this is our profile, we have to see ALL informations that we can edit. This means that we don't need to check the public masks for sending
                // the data

                datatoSend = string.Format(datatoSend + @"\nick\{0}\uniquenick\{1}\email\{2}\id\{3}\pmask\{4}", PlayerNick, PlayerUniqueNick, PlayerEmail, /*(ProfileSent ? "5" : "2")*/ messID, PlayerPublicMask);

                if (PlayerLastName.Length > 0)
                    datatoSend += @"\lastname\" + PlayerLastName;

                if (PlayerFirstName.Length > 0)
                    datatoSend += @"\firstname\" + PlayerFirstName;

                if (PlayerICQ != 0)
                    datatoSend += @"\icquin\" + PlayerICQ;

                if (PlayerHomepage.Length > 0)
                    datatoSend += @"\homepage\" + PlayerHomepage;

                if (PlayerPicture != 0)
                    datatoSend += @"\pic\" + PlayerPicture;

                if (PlayerAim.Length > 0)
                    datatoSend += @"\aim\" + PlayerAim;

                if (PlayerOccupation != 0)
                    datatoSend += @"\occ\" + PlayerOccupation;

                if (PlayerZIPCode.Length > 0)
                    datatoSend += @"\zipcode\" + PlayerZIPCode;

                if (PlayerCountryCode.Length > 0)
                    datatoSend += @"\countrycode\" + PlayerCountryCode;

                if (PlayerBirthday > 0 && PlayerBirthmonth > 0 && PlayerBirthyear > 0)
                    datatoSend += @"\birthday\" + (uint)((PlayerBirthday << 24) | (PlayerBirthmonth << 16) | PlayerBirthyear);

                if (PlayerLocation.Length > 0)
                    datatoSend += @"\loc\" + PlayerLocation;

                if (PlayerSex == PlayerSexType.FEMALE)
                    datatoSend += @"\sex\1";
                else if (PlayerSex == PlayerSexType.MALE)
                    datatoSend += @"\sex\0";

                if (PlayerLatitude != 0.0f)
                    datatoSend += @"\lat\" + PlayerLatitude;

                if (PlayerLongitude != 0.0f)
                    datatoSend += @"\lon\" + PlayerLongitude;

                if (PlayerIncomeID != 0)
                    datatoSend += @"\inc\" + PlayerIncomeID;

                if (PlayerIndustryID != 0)
                    datatoSend += @"\ind\" + PlayerIndustryID;

                if (PlayerMarried != 0)
                    datatoSend += @"\mar\" + PlayerMarried;

                if (PlayerChildCount != 0)
                    datatoSend += @"\chc\" + PlayerChildCount;

                if (PlayerInterests != 0)
                    datatoSend += @"\i1\" + PlayerInterests;

                if (PlayerOwnership != 0)
                    datatoSend += @"\o1\" + PlayerOwnership;

                if (PlayerConnectionType != 0)
                    datatoSend += @"\conn\" + PlayerConnectionType;

                // SUPER NOTE: Please check the Signature of the PID, otherwise when it will be compared with other peers, it will break everything (See gpiPeer.c @ peerSig)
                datatoSend += @"\sig\" + GameSpyLib.Random.GenerateRandomString(33, GameSpyLib.Random.StringType.Hex) + @"\final\";

                // Set that we send the profile initially
                if (!ProfileSent) ProfileSent = true;
            }

            Stream.SendAsync(datatoSend);
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
                LogWriter.Log.WriteException(e);
            }
        }

        /// <summary>
        /// Polls the connection, and checks for drops
        /// </summary>
        public void SendKeepAlive()
        {
            if (LoginStatus == LoginStatus.Completed)
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
        /// <param name="userdata">The user data to append to the proof</param>
        /// <param name="partnerid">The partnerid to append</param>
        /// <returns>
        ///     The proof verification MD5 hash string that can be compared to what the client sends,
        ///     to verify that the users entered password matches the specific user data in the database.
        /// </returns>
        private string GenerateProof(string challenge1, string challenge2, string userdata, uint partnerid)
        {
            string realUserData = userdata;

            if (partnerid != (uint)PartnerID.Gamespy)
            {
                realUserData = string.Format("{0}@{1}", partnerid, userdata);
            }

            // Generate our string to be hashed
            StringBuilder HashString = new StringBuilder(PasswordHash);
            HashString.Append(' ', 48); // 48 spaces
            HashString.Append(realUserData);
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
