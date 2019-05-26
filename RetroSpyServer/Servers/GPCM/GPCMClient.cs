using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using GameSpyLib.Logging;
using GameSpyLib.Database;
using GameSpyLib.Network;
using GameSpyLib.Common;
using GameSpyLib.Extensions;
using RetroSpyServer.Application;
using RetroSpyServer.DBQueries;

namespace RetroSpyServer.Servers.GPCM
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
        public PublicMasks PlayerPublicMask { get; protected set; }
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
        public GameSpyTCPHandler Stream { get; protected set; }

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

        private GPCMDBQuery gPCMDBQuery;

        #endregion Variables

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ReadArgs">The Tcp Client connection</param>
        public GPCMClient(GameSpyTCPHandler ConnectionStream, long ConnectionId, DatabaseDriver driver)
        {
            // Set default variable values
             gPCMDBQuery = new GPCMDBQuery(driver);

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
            

            // Create our Client Stream
            Stream = ConnectionStream;
            Stream.OnDisconnect += Stream_OnDisconnect;
            Stream.DataReceived += Stream_DataReceived;
            Stream.IsMessageFinished += Stream_IsMessageFinished;
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
        /// Check if a date is correct
        /// </summary>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>True if the date is valid, otherwise false</returns>
        protected bool IsValidDate(ushort day, ushort month, ushort year)
        {
            // Check for a blank.
            /////////////////////
            if ((day == 0) && (month == 0) && (year == 0))
                return false;

            // Validate the day of the month.
            /////////////////////////////////
            switch (month)
            {
                // No month.
                ////////////
                case 0:
                    // Can't specify a day without a month.
                    ///////////////////////////////////////
                    if (day != 0)
                        return false;
                    break;

                // 31-day month.
                ////////////////
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    if (day > 31)
                        return false;
                    break;

                // 30-day month.
                ////////////////
                case 4:
                case 6:
                case 9:
                case 11:
                    if (day > 30)
                        return false;
                    break;

                // 28/29-day month.
                ///////////////////
                case 2:
                    // Leap year?
                    /////////////
                    if ((((year % 4) == 0) && ((year % 100) != 0)) || ((year % 400) == 0))
                    {
                        if (day > 29)
                            return false;
                    }
                    else
                    {
                        if (day > 28)
                            return false;
                    }
                    break;

                // Invalid month.
                /////////////////
                default:
                    return false;
            }

            // Check that the date is in the valid range.
            /////////////////////////////////////////////
            if (year < 1900)
                return false;
            if (year > 2079)
                return false;
            if (year == 2079)
            {
                if (month > 6)
                    return false;
                if ((month == 6) && (day > 6))
                    return false;
            }

            return true;
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
                Stream.OnDisconnect -= Stream_OnDisconnect;
                Stream.DataReceived -= Stream_DataReceived;
                Stream.IsMessageFinished -= Stream_IsMessageFinished;
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

        private bool Stream_IsMessageFinished(string message)
        {
            if (message.EndsWith("\\final\\"))
                return true;

            return false;
        }

        /// <summary>
        /// Main listner loop. Keeps an open stream between the client and server while
        /// the client is logged in / playing
        /// </summary>
        private void Stream_DataReceived(string message)
        {
            if (message[0] != '\\')
            {
                GamespyUtils.SendGPError(Stream, 0, "An invalid request was sended.");
                return;
            }

            string[] submessage = message.Split("\\final\\");

            foreach (string command in submessage)
            {
                // Read client message, and parse it into key value pairs
                string[] recieved = command.TrimStart('\\').Split('\\');

                switch (recieved[0])
                {
                    case "inviteto":
                        AddProducts(GamespyUtils.ConvertGPResponseToKeyValue(recieved));
                        break;
                    case "newuser":
                        CreateNewUser(GamespyUtils.ConvertGPResponseToKeyValue(recieved));
                        break;
                    case "login":
                        ProcessLogin(GamespyUtils.ConvertGPResponseToKeyValue(recieved));
                        break;
                    case "getprofile":
                        SendProfile(GamespyUtils.ConvertGPResponseToKeyValue(recieved));
                        break;
                    case "updatepro":
                        UpdateUser(GamespyUtils.ConvertGPResponseToKeyValue(recieved));
                        break;
                    case "logout":
                        Disconnect(DisconnectReason.NormalLogout);
                        break;
                    case "status":
                        UpdateStatus(GamespyUtils.ConvertGPResponseToKeyValue(recieved));
                        break;
                    case "ka":
                        SendKeepAlive();
                        break;
                    default:
                        LogWriter.Log.Write("Received unknown request " + recieved[0], LogLevel.Debug);
                        GamespyUtils.SendGPError(Stream, 0, "An invalid request was sended.");
                        break;
                }
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
            ushort testSK;

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
        private void Stream_OnDisconnect()
        {
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
            ServerChallengeKey = GameSpyLib.Common.Random.GenerateRandomString(10, GameSpyLib.Common.Random.StringType.Alpha);
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
                GamespyUtils.SendGPError(Stream, 0, "Invalid response received from the client!");
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
                        QueryResult = gPCMDBQuery.GetUserFromUniqueNick(Recv["uniquenick"]);
                    else if (PlayerAuthToken != null)
                    {
                        //TODO! Add the database entry
                        GamespyUtils.SendGPError(Stream, 0, "AuthToken is not supported yet");
                        return;
                    }
                    else
                        QueryResult = gPCMDBQuery.GetUserFromNickname(PlayerEmail, PlayerNick);
                }
                catch (Exception)
                {
                    GamespyUtils.SendGPError(Stream, 4, "This request cannot be processed because of a database error.");
                    return;
                }

                if (QueryResult == null)
                {
                    if (PlayerUniqueNick != null)
                        GamespyUtils.SendGPError(Stream, 265, "The unique nickname provided is incorrect!");
                    else
                        GamespyUtils.SendGPError(Stream, 265, "The nickname provided is incorrect!");

                    Disconnect(DisconnectReason.InvalidUsername);
                    return;
                }

                // Check if user is banned
                PlayerStatus currentPlayerStatus;
                UserStatus currentUserStatus;

                if (!Enum.TryParse(QueryResult["status"].ToString(), out currentPlayerStatus))
                {
                    GamespyUtils.SendGPError(Stream, 265, "Invalid player data! Please contact an administrator.");
                    Disconnect(DisconnectReason.InvalidPlayer);
                    return;
                }

                if (!Enum.TryParse(QueryResult["userstatus"].ToString(), out currentUserStatus))
                {
                    GamespyUtils.SendGPError(Stream, 265, "Invalid player data! Please contact an administrator.");
                    Disconnect(DisconnectReason.InvalidPlayer);
                    return;
                }

                // Check the status of the account.
                // If the single profile is banned, the account or the player status

                if (currentPlayerStatus == PlayerStatus.Banned)
                {
                    GamespyUtils.SendGPError(Stream, 265, "Your profile has been permanently suspended.");
                    Disconnect(DisconnectReason.PlayerIsBanned);
                    return;
                }

                if (currentUserStatus == UserStatus.Created)
                {
                    GamespyUtils.SendGPError(Stream, 265, "Your account is not verified. Please check your email inbox and verify the account.");
                    Disconnect(DisconnectReason.PlayerIsBanned);
                    return;
                }

                if (currentUserStatus == UserStatus.Banned)
                {
                    GamespyUtils.SendGPError(Stream, 265, "Your account has been permanently suspended.");
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

                PublicMasks mask;
                if (!Enum.TryParse(QueryResult["publicmask"].ToString(), out mask))
                    PlayerPublicMask = PublicMasks.MASK_ALL;
                else
                    PlayerPublicMask = mask;

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
                        GameSpyLib.Common.Random.GenerateRandomString(22, GameSpyLib.Common.Random.StringType.Hex) // Generate LT whatever that is (some sort of random string, 22 chars long)
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
                GamespyUtils.SendGPError(Stream, 1, "There was an error parsing an incoming request.");
                return;
            }

            uint targetPID, messID;
            if (!uint.TryParse(dict["profileid"], out targetPID))
            {
                GamespyUtils.SendGPError(Stream, 1, "There was an error parsing an incoming request.");
                return;
            }

            if (!uint.TryParse(dict["id"], out messID))
            {
                GamespyUtils.SendGPError(Stream, 1, "There was an error parsing an incoming request.");
                return;
            }

            string datatoSend = @"\pi\\profileid\" + targetPID + @"\mp\4";

            // If the client want to access the public information
            // of another client
            if (targetPID != PlayerId)
            {
                uint publicMask;

                var Query = gPCMDBQuery.GetProfileInfo(targetPID);
                if (Query == null)
                {
                    GamespyUtils.SendGPError(Stream, 4, "Unable to get profile information.");
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
                        if (sexType == PlayerSexType.FEMALE)
                            datatoSend += @"\sex\1";
                        else if (sexType == PlayerSexType.MALE)
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
                datatoSend += @"\sig\" + GameSpyLib.Common.Random.GenerateRandomString(33, GameSpyLib.Common.Random.StringType.Hex) + @"\final\";
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
                datatoSend += @"\sig\" + GameSpyLib.Common.Random.GenerateRandomString(33, GameSpyLib.Common.Random.StringType.Hex) + @"\final\";

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
                if (gPCMDBQuery.UserExists(Recv["nick"]))
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
                if ((PlayerId = gPCMDBQuery.CreateUser(Recv["nick"], Password, Recv["email"], Cc, Recv["nick"])) == 0)
                {
                    GamespyUtils.SendGPError(Stream, 516, "An error oncurred while creating the account!");
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
                    GamespyUtils.SendGPError(Stream, 516, "Invalid response received from the client!");
                }
                else
                {
                    GamespyUtils.SendGPError(Stream, 516, "An error oncurred while creating the account!");
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
            if (!Recv.ContainsKey("sesskey"))
                return;

            ushort ssk;
            if (!ushort.TryParse(Recv["sesskey"], out ssk))
                return;

            if (ssk != SessionKey)
                return;

            string query = "UPDATE profiles SET";
            object[] passData = new object[22] {
                null, // publicmask : 0
                null, // firstname
                null, // lastname
                null, // icq
                null, // homepage
                null, // zipcode
                null, // countrycode
                null, // birthday
                null, // sex
                null, // aim
                null, // pic
                null, // occ
                null, // ind
                null, // inc
                null, // mar
                null, // chc
                null, // i1
                null, // nick
                null, // uniquenick
                null, // Bithmonth
                null, // Birthyear
                null  // ProfileID
            };

            if (Recv.ContainsKey("publicmask"))
            {
                PublicMasks mask;
                if (Enum.TryParse(Recv["publicmask"], out mask))
                {
                    if (PlayerPublicMask != mask)
                    {
                        query += ", publicmask=@P0";
                        PlayerPublicMask = mask;
                        passData[0] = mask;
                    }
                }
            }

            if (Recv.ContainsKey("firstname"))
            {
                if (Recv["firstname"] != PlayerFirstName)
                {
                    query += ", firstname=@P1";
                    PlayerFirstName = Recv["firstname"];
                    passData[1] = PlayerFirstName;
                }
            }

            if (Recv.ContainsKey("lastname"))
            {
                if (Recv["lastname"] != PlayerLastName)
                {
                    query += ", lastname=@P2";
                    PlayerFirstName = Recv["lastname"];
                    passData[2] = PlayerLastName;
                }
            }

            if (Recv.ContainsKey("icquin"))
            {
                int icq = 0;

                if (int.TryParse(Recv["icquin"], out icq))
                {
                    if (icq != PlayerICQ)
                    {
                        query += "icq=@P3 ";
                        PlayerICQ = icq;
                        passData[3] = icq;
                    }
                }
            }

            if (Recv.ContainsKey("homepage"))
            {
                if (Recv["homepage"] != PlayerHomepage)
                {
                    query += ", homepage=@P4";
                    PlayerHomepage = Recv["homepage"];
                    passData[4] = PlayerHomepage;
                }
            }

            if (Recv.ContainsKey("zipcode"))
            {
                if (Recv["zipcode"] != PlayerZIPCode)
                {
                    query += ", zipcode=@P5";
                    PlayerZIPCode = Recv["zipcode"];
                    passData[5] = PlayerZIPCode;
                }
            }

            if (Recv.ContainsKey("countrycode"))
            {
                if (Recv["countrycode"] != PlayerCountryCode)
                {
                    query += ", countrycode=@P6";
                    PlayerCountryCode = Recv["zipcode"];
                    passData[6] = PlayerCountryCode;
                }
            }

            if (Recv.ContainsKey("birthday"))
            {
                int date;
                if (int.TryParse(Recv["birthday"], out date))
                {
                    ushort d = (ushort)((date >> 24) & 0xFF);
                    ushort m = (ushort)((date >> 16) & 0xFF);
                    ushort y = (ushort)(date & 0xFFFF);

                    if (IsValidDate(d, m, y))
                    {
                        if (PlayerBirthday != d)
                        {
                            query += ", birthday=@P6";
                            passData[6] = d;
                            PlayerBirthday = d;
                        }

                        if (PlayerBirthmonth != m)
                        {
                            query += ", birthmonth=@P19";
                            passData[19] = m;
                            PlayerBirthmonth = m;
                        }

                        if (PlayerBirthyear != y)
                        {
                            query += ", birthyear=@P20";
                            passData[20] = y;
                            PlayerBirthyear = y;
                        }
                    }
                }


                if (Recv["countrycode"] != PlayerCountryCode)
                {
                    query += ", countrycode=@P7";
                    PlayerCountryCode = Recv["zipcode"];
                    passData[7] = PlayerCountryCode;
                }
            }


            if (Recv.ContainsKey("sex"))
            {
                PlayerSexType sex;
                if (Enum.TryParse(Recv["sex"], out sex))
                {
                    if (PlayerSex != sex)
                    {
                        query += "sex=@P8";
                        PlayerSex = sex;

                        if (PlayerSex == PlayerSexType.MALE)
                            passData[8] = "MALE";
                        else if (PlayerSex == PlayerSexType.FEMALE)
                            passData[8] = "FEMALE";
                        else
                            passData[8] = "PAT";
                    }
                }
            }

            if (Recv.ContainsKey("aim"))
            {
                if (Recv["aim"] != PlayerAim)
                {
                    query += ", aim=@P9";
                    PlayerAim = Recv["aim"];
                    passData[9] = PlayerAim;
                }
            }

            if (Recv.ContainsKey("pic"))
            {
                int pic = 0;

                if (int.TryParse(Recv["pic"], out pic))
                {
                    if (pic != PlayerPicture)
                    {
                        query += ", picture=@P10";
                        PlayerPicture = pic;
                        passData[10] = pic;
                    }
                }
            }

            if (Recv.ContainsKey("occ"))
            {
                int occ = 0;

                if (int.TryParse(Recv["occ"], out occ))
                {
                    if (occ != PlayerOccupation)
                    {
                        query += ", occupationid=@P11";
                        PlayerOccupation = occ;
                        passData[11] = occ;
                    }
                }
            }

            if (Recv.ContainsKey("ind"))
            {
                int ind = 0;

                if (int.TryParse(Recv["ind"], out ind))
                {
                    if (ind != PlayerIndustryID)
                    {
                        query += ", industryid=@P12";
                        PlayerIndustryID = ind;
                        passData[12] = ind;
                    }
                }
            }

            if (Recv.ContainsKey("inc"))
            {
                int inc = 0;

                if (int.TryParse(Recv["inc"], out inc))
                {
                    if (inc != PlayerIncomeID)
                    {
                        query += ", industryid=@P13";
                        PlayerIncomeID = inc;
                        passData[13] = inc;
                    }
                }
            }

            if (Recv.ContainsKey("mar"))
            {
                int mar = 0;

                if (int.TryParse(Recv["mar"], out mar))
                {
                    if (mar != PlayerMarried)
                    {
                        query += ", marriedid=@P14";
                        PlayerMarried = mar;
                        passData[14] = mar;
                    }
                }
            }

            if (Recv.ContainsKey("chc"))
            {
                int chc = 0;

                if (int.TryParse(Recv["chc"], out chc))
                {
                    if (chc != PlayerChildCount)
                    {
                        query += ", childcount=@P15";
                        PlayerChildCount = chc;
                        passData[15] = chc;
                    }
                }
            }

            if (Recv.ContainsKey("i1"))
            {
                int i1 = 0;

                if (int.TryParse(Recv["i1"], out i1))
                {
                    if (i1 != PlayerInterests)
                    {
                        query += ", interests1=@P16";
                        PlayerInterests = i1;
                        passData[16] = i1;
                    }
                }
            }

            if (Recv.ContainsKey("nick"))
            {
                if (Recv["nick"] != PlayerNick)
                {
                    query += ", nick=@P17";
                    PlayerNick = Recv["nick"];
                    passData[17] = PlayerNick;
                }
            }

            if (Recv.ContainsKey("uniquenick"))
            {
                if (Recv["uniquenick"] != PlayerUniqueNick)
                {
                    query += ", uniquenick=@P18";
                    PlayerHomepage = Recv["uniquenick"];
                    passData[18] = PlayerUniqueNick;
                }
            }

            if (query == "UPDATE profiles SET")
                return;

            query = query.Replace("SET,", "SET");

            passData[21] = PlayerId;
            query += " WHERE `profileid`=@P21";

            try
            {
                gPCMDBQuery.Query(query, passData);
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
