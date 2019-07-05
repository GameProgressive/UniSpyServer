using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using GameSpyLib.Common;
using GameSpyLib.Extensions;
using RetroSpyServer.Application;
using RetroSpyServer.Servers.GPCM.Enumerator;
using RetroSpyServer.Servers.GPCM.Structures;
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
        /// Indicates whether this player successfully completed the login process
        /// </summary>
        public bool CompletedLoginProcess { get; protected set; } = false;

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

        public GPCMPlayerInfo PlayerInfo { get; protected set; }
        

        #endregion Variables

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ReadArgs">The Tcp Client connection</param>
        public GPCMClient(TcpStream ConnectionStream, long ConnectionId)
        {
            PlayerInfo = new GPCMPlayerInfo();

            RemoteEndPoint = (IPEndPoint)ConnectionStream.RemoteEndPoint;
            Disposed = false;

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

        public void SendServerChallenge(uint ServerID)
        {
            // Only send the login challenge once
            if (PlayerInfo.LoginStatus != LoginStatus.Connected)
            {
                Disconnect(DisconnectReason.ClientChallengeAlreadySent);

                // Throw the error
                //throw new Exception("The server challenge has already been sent. Cannot send another login challenge." + $"\tChallenge was sent \"{ts.ToString()}\" ago.");
                //Undo this
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
                Stream.OnDisconnect -= Stream_OnDisconnect;
                Stream.DataReceived -= Stream_DataReceived;
                Stream.IsMessageFinished -= Stream_IsMessageFinished;
                Stream.Close(reason == DisconnectReason.ForcedServerShutdown);
            }
            catch { }

            // Set status and log
            if (PlayerInfo.LoginStatus == LoginStatus.Completed)
            {
                if (reason == DisconnectReason.NormalLogout)
                {
                    LogWriter.Log.Write(
                        "Client Logout:  {0} - {1} - {2}",
                        LogLevel.Info,
                        PlayerInfo.PlayerNick,
                        PlayerInfo.PlayerId,
                        RemoteEndPoint
                    );
                }
                else if (reason != DisconnectReason.ForcedServerShutdown)
                {
                    LogWriter.Log.Write(
                        "Client Disconnected:  {0} - {1} - {2}, Code={3}",
                        LogLevel.Info,
                        PlayerInfo.PlayerNick,
                        PlayerInfo.PlayerId,
                        RemoteEndPoint,
                        Enum.GetName(typeof(DisconnectReason), reason)
                    );
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

        private bool Stream_IsMessageFinished(string message)
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

            PlayerInfo.PlayerStatusString = dictionary["statstring"];
            PlayerInfo.PlayerLocation = dictionary["locstring"];

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
                PlayerInfo.PlayerUniqueNick = Recv["uniquenick"];
            }
            else if (Recv.ContainsKey("authtoken"))
            {
                PlayerInfo.PlayerAuthToken = Recv["authtoken"];
            }
            else if (Recv.ContainsKey("user"))
            {
                // "User" is <nickname>@<email>
                string User = Recv["user"];
                int Pos = User.IndexOf('@');
                PlayerInfo.PlayerNick = User.Substring(0, Pos);
                PlayerInfo.PlayerEmail = User.Substring(Pos + 1);
            }

            // Dispose connection after use
            try
            {
                // Try and fetch the user from the database
                Dictionary<string, object> QueryResult;

                try
                {
                    if (PlayerInfo.PlayerUniqueNick != null)
                        QueryResult = GPCMHelper.DBQuery.GetUserFromUniqueNick(Recv["uniquenick"]);
                    else if (PlayerInfo.PlayerAuthToken != null)
                    {
                        //TODO! Add the database entry
                        GamespyUtils.SendGPError(Stream, 0, "AuthToken is not supported yet");
                        return;
                    }
                    else
                        QueryResult = GPCMHelper.DBQuery.GetUserFromNickname(PlayerInfo.PlayerEmail, PlayerInfo.PlayerNick);
                }
                catch (Exception)
                {
                    GamespyUtils.SendGPError(Stream, 4, "This request cannot be processed because of a database error.");
                    return;
                }

                if (QueryResult == null)
                {
                    if (PlayerInfo.PlayerUniqueNick != null)
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
                PlayerInfo.PlayerId = uint.Parse(QueryResult["profileid"].ToString());
                PlayerInfo.PasswordHash = QueryResult["password"].ToString().ToLowerInvariant();
                PlayerInfo.PlayerCountryCode = QueryResult["countrycode"].ToString();

                PlayerInfo.PlayerFirstName = QueryResult["firstname"].ToString();
                PlayerInfo.PlayerLastName = QueryResult["lastname"].ToString();
                PlayerInfo.PlayerICQ = int.Parse(QueryResult["icq"].ToString());
                PlayerInfo.PlayerHomepage = QueryResult["homepage"].ToString();
                PlayerInfo.PlayerZIPCode = QueryResult["zipcode"].ToString();
                PlayerInfo.PlayerLocation = QueryResult["location"].ToString();
                PlayerInfo.PlayerAim = QueryResult["aim"].ToString();
                PlayerInfo.PlayerOwnership = int.Parse(QueryResult["ownership1"].ToString());
                PlayerInfo.PlayerOccupation = int.Parse(QueryResult["occupationid"].ToString());
                PlayerInfo.PlayerIndustryID = int.Parse(QueryResult["industryid"].ToString());
                PlayerInfo.PlayerIncomeID = int.Parse(QueryResult["incomeid"].ToString());
                PlayerInfo.PlayerMarried = int.Parse(QueryResult["marriedid"].ToString());
                PlayerInfo.PlayerChildCount = int.Parse(QueryResult["childcount"].ToString());
                PlayerInfo.PlayerConnectionType = int.Parse(QueryResult["connectiontype"].ToString());
                PlayerInfo.PlayerPicture = int.Parse(QueryResult["picture"].ToString());
                PlayerInfo.PlayerInterests = int.Parse(QueryResult["interests1"].ToString());
                PlayerInfo.PlayerBirthday = ushort.Parse(QueryResult["birthday"].ToString());
                PlayerInfo.PlayerBirthmonth = ushort.Parse(QueryResult["birthmonth"].ToString());
                PlayerInfo.PlayerBirthyear = ushort.Parse(QueryResult["birthyear"].ToString());

                PlayerSexType playerSexType;
                if (!Enum.TryParse(QueryResult["sex"].ToString().ToUpper(), out playerSexType))
                    PlayerInfo.PlayerSex = PlayerSexType.PAT;
                else
                    PlayerInfo.PlayerSex = playerSexType;

                PlayerInfo.PlayerLatitude = float.Parse(QueryResult["latitude"].ToString());
                PlayerInfo.PlayerLongitude = float.Parse(QueryResult["longitude"].ToString());

                PublicMasks mask;
                if (!Enum.TryParse(QueryResult["publicmask"].ToString(), out mask))
                    PlayerInfo.PlayerPublicMask = PublicMasks.MASK_ALL;
                else
                    PlayerInfo.PlayerPublicMask = mask;

                string challengeData = "";

                if (PlayerInfo.PlayerUniqueNick != null)
                {
                    PlayerInfo.PlayerEmail = QueryResult["email"].ToString();
                    PlayerInfo.PlayerNick = QueryResult["nick"].ToString();
                    challengeData = PlayerInfo.PlayerUniqueNick;
                }
                else if (PlayerInfo.PlayerAuthToken != null)
                {
                    PlayerInfo.PlayerEmail = QueryResult["email"].ToString();
                    PlayerInfo.PlayerNick = QueryResult["nick"].ToString();
                    PlayerInfo.PlayerUniqueNick = QueryResult["uniquenick"].ToString();
                    challengeData = PlayerInfo.PlayerAuthToken;
                }
                else
                {
                    PlayerInfo.PlayerUniqueNick = QueryResult["uniquenick"].ToString();
                    challengeData = Recv["user"];
                }

                // Use the GenerateProof method to compare with the "response" value. This validates the given password
                if (Recv["response"] == GenerateProof(Recv["challenge"], ServerChallengeKey, challengeData, PlayerInfo.PlayerAuthToken != null ? 0 : partnerID))
                {
                    // Create session key
                    SessionKey = Crc.ComputeChecksum(PlayerInfo.PlayerUniqueNick);

                    // Password is correct
                    Stream.SendAsync(
                        @"\lc\2\sesskey\{0}\proof\{1}\userid\{2}\profileid\{2}\uniquenick\{3}\lt\{4}__\id\1\final\",
                        SessionKey,
                        GenerateProof(ServerChallengeKey, Recv["challenge"], challengeData, PlayerInfo.PlayerAuthToken != null ? 0 : partnerID), // Do this again, Params are reversed!
                        PlayerInfo.PlayerId,
                        PlayerInfo.PlayerNick,
                        GameSpyLib.Common.Random.GenerateRandomString(22, GameSpyLib.Common.Random.StringType.Hex) // Generate LT whatever that is (some sort of random string, 22 chars long)
                    );

                    // Log Incoming Connections
                    LogWriter.Log.Write("Client Login:   {0} - {1} - {2}", LogLevel.Info, PlayerInfo.PlayerNick, PlayerInfo.PlayerId, RemoteEndPoint);

                    // Update status last, and call success login
                    PlayerInfo.LoginStatus = LoginStatus.Completed;
                    PlayerInfo.PlayerStatus = PlayerStatus.Online;
                    PlayerInfo.PlayerStatusString = "Online";
                    PlayerInfo.PlayerStatusLocation = "";

                    CompletedLoginProcess = true;
                    OnSuccessfulLogin?.Invoke(this);
                    OnStatusChanged?.Invoke(this);

                    SendBuddies();
                }
                else
                {
                    // Log Incoming Connections
                    LogWriter.Log.Write("Failed Login Attempt: {0} - {1} - {2}", LogLevel.Info, PlayerInfo.PlayerNick, PlayerInfo.PlayerId, RemoteEndPoint);

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
            if (targetPID != PlayerInfo.PlayerId)
            {
                uint publicMask;

                var Query = GPCMHelper.DBQuery.GetProfileInfo(targetPID);
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

                if (PlayerInfo.PlayerHomepage.Length > 0 && publicMask != (uint)PublicMasks.MASK_NONE)
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

                datatoSend = string.Format(datatoSend + @"\nick\{0}\uniquenick\{1}\email\{2}\id\{3}\pmask\{4}", 
                                                                PlayerInfo.PlayerNick, 
                                                                PlayerInfo.PlayerUniqueNick, 
                                                                PlayerInfo.PlayerEmail,
                                                                /*(ProfileSent ? "5" : "2")*/ messID, 
                                                                PlayerInfo.PlayerPublicMask
                                                                );

                if (PlayerInfo.PlayerLastName.Length > 0)
                    datatoSend += @"\lastname\" + PlayerInfo.PlayerLastName;

                if (PlayerInfo.PlayerFirstName.Length > 0)
                    datatoSend += @"\firstname\" + PlayerInfo.PlayerFirstName;

                if (PlayerInfo.PlayerICQ != 0)
                    datatoSend += @"\icquin\" + PlayerInfo.PlayerICQ;

                if (PlayerInfo.PlayerHomepage.Length > 0)
                    datatoSend += @"\homepage\" + PlayerInfo.PlayerHomepage;

                if (PlayerInfo.PlayerPicture != 0)
                    datatoSend += @"\pic\" + PlayerInfo.PlayerPicture;

                if (PlayerInfo.PlayerAim.Length > 0)
                    datatoSend += @"\aim\" + PlayerInfo.PlayerAim;

                if (PlayerInfo.PlayerOccupation != 0)
                    datatoSend += @"\occ\" + PlayerInfo.PlayerOccupation;

                if (PlayerInfo.PlayerZIPCode.Length > 0)
                    datatoSend += @"\zipcode\" + PlayerInfo.PlayerZIPCode;

                if (PlayerInfo.PlayerCountryCode.Length > 0)
                    datatoSend += @"\countrycode\" + PlayerInfo.PlayerCountryCode;

                if (PlayerInfo.PlayerBirthday > 0 && PlayerInfo.PlayerBirthmonth > 0 && PlayerInfo.PlayerBirthyear > 0)
                    datatoSend += @"\birthday\" + (uint)((PlayerInfo.PlayerBirthday << 24) | (PlayerInfo.PlayerBirthmonth << 16) | PlayerInfo.PlayerBirthyear);

                if (PlayerInfo.PlayerLocation.Length > 0)
                    datatoSend += @"\loc\" + PlayerInfo.PlayerLocation;

                if (PlayerInfo.PlayerSex == PlayerSexType.FEMALE)
                    datatoSend += @"\sex\1";
                else if (PlayerInfo.PlayerSex == PlayerSexType.MALE)
                    datatoSend += @"\sex\0";

                if (PlayerInfo.PlayerLatitude != 0.0f)
                    datatoSend += @"\lat\" + PlayerInfo.PlayerLatitude;

                if (PlayerInfo.PlayerLongitude != 0.0f)
                    datatoSend += @"\lon\" + PlayerInfo.PlayerLongitude;

                if (PlayerInfo.PlayerIncomeID != 0)
                    datatoSend += @"\inc\" + PlayerInfo.PlayerIncomeID;

                if (PlayerInfo.PlayerIndustryID != 0)
                    datatoSend += @"\ind\" + PlayerInfo.PlayerIndustryID;

                if (PlayerInfo.PlayerMarried != 0)
                    datatoSend += @"\mar\" + PlayerInfo.PlayerMarried;

                if (PlayerInfo.PlayerChildCount != 0)
                    datatoSend += @"\chc\" + PlayerInfo.PlayerChildCount;

                if (PlayerInfo.PlayerInterests != 0)
                    datatoSend += @"\i1\" + PlayerInfo.PlayerInterests;

                if (PlayerInfo.PlayerOwnership != 0)
                    datatoSend += @"\o1\" + PlayerInfo.PlayerOwnership;

                if (PlayerInfo.PlayerConnectionType != 0)
                    datatoSend += @"\conn\" + PlayerInfo.PlayerConnectionType;

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
                if (GPCMHelper.DBQuery.UserExists(Recv["nick"]))
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
                if ((PlayerInfo.PlayerId = GPCMHelper.DBQuery.CreateUser(Recv["nick"], Password, Recv["email"], Cc, Recv["nick"])) == 0)
                {
                    GamespyUtils.SendGPError(Stream, 516, "An error oncurred while creating the account!");
                    Disconnect(DisconnectReason.CreateFailedDatabaseError);
                    return;
                }

                Stream.SendAsync(@"\nur\\userid\{0}\profileid\{0}\id\1\final\", PlayerInfo.PlayerId);
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
                    if (PlayerInfo.PlayerPublicMask != mask)
                    {
                        query += ", publicmask=@P0";
                        PlayerInfo.PlayerPublicMask = mask;
                        passData[0] = mask;
                    }
                }
            }

            if (Recv.ContainsKey("firstname"))
            {
                if (Recv["firstname"] != PlayerInfo.PlayerFirstName)
                {
                    query += ", firstname=@P1";
                    PlayerInfo.PlayerFirstName = Recv["firstname"];
                    passData[1] = PlayerInfo.PlayerFirstName;
                }
            }

            if (Recv.ContainsKey("lastname"))
            {
                if (Recv["lastname"] != PlayerInfo.PlayerLastName)
                {
                    query += ", lastname=@P2";
                    PlayerInfo.PlayerFirstName = Recv["lastname"];
                    passData[2] = PlayerInfo.PlayerLastName;
                }
            }

            if (Recv.ContainsKey("icquin"))
            {
                int icq = 0;

                if (int.TryParse(Recv["icquin"], out icq))
                {
                    if (icq != PlayerInfo.PlayerICQ)
                    {
                        query += "icq=@P3 ";
                        PlayerInfo.PlayerICQ = icq;
                        passData[3] = icq;
                    }
                }
            }

            if (Recv.ContainsKey("homepage"))
            {
                if (Recv["homepage"] != PlayerInfo.PlayerHomepage)
                {
                    query += ", homepage=@P4";
                    PlayerInfo.PlayerHomepage = Recv["homepage"];
                    passData[4] = PlayerInfo.PlayerHomepage;
                }
            }

            if (Recv.ContainsKey("zipcode"))
            {
                if (Recv["zipcode"] != PlayerInfo.PlayerZIPCode)
                {
                    query += ", zipcode=@P5";
                    PlayerInfo.PlayerZIPCode = Recv["zipcode"];
                    passData[5] = PlayerInfo.PlayerZIPCode;
                }
            }

            if (Recv.ContainsKey("countrycode"))
            {
                if (Recv["countrycode"] != PlayerInfo.PlayerCountryCode)
                {
                    query += ", countrycode=@P6";
                    PlayerInfo.PlayerCountryCode = Recv["zipcode"];
                    passData[6] = PlayerInfo.PlayerCountryCode;
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

                    if (GamespyUtils.IsValidDate(d, m, y))
                    {
                        if (PlayerInfo.PlayerBirthday != d)
                        {
                            query += ", birthday=@P6";
                            passData[6] = d;
                            PlayerInfo.PlayerBirthday = d;
                        }

                        if (PlayerInfo.PlayerBirthmonth != m)
                        {
                            query += ", birthmonth=@P19";
                            passData[19] = m;
                            PlayerInfo.PlayerBirthmonth = m;
                        }

                        if (PlayerInfo.PlayerBirthyear != y)
                        {
                            query += ", birthyear=@P20";
                            passData[20] = y;
                            PlayerInfo.PlayerBirthyear = y;
                        }
                    }
                }


                if (Recv["countrycode"] != PlayerInfo.PlayerCountryCode)
                {
                    query += ", countrycode=@P7";
                    PlayerInfo.PlayerCountryCode = Recv["zipcode"];
                    passData[7] = PlayerInfo.PlayerCountryCode;
                }
            }


            if (Recv.ContainsKey("sex"))
            {
                PlayerSexType sex;
                if (Enum.TryParse(Recv["sex"], out sex))
                {
                    if (PlayerInfo.PlayerSex != sex)
                    {
                        query += "sex=@P8";
                        PlayerInfo.PlayerSex = sex;

                        if (PlayerInfo.PlayerSex == PlayerSexType.MALE)
                            passData[8] = "MALE";
                        else if (PlayerInfo.PlayerSex == PlayerSexType.FEMALE)
                            passData[8] = "FEMALE";
                        else
                            passData[8] = "PAT";
                    }
                }
            }

            if (Recv.ContainsKey("aim"))
            {
                if (Recv["aim"] != PlayerInfo.PlayerAim)
                {
                    query += ", aim=@P9";
                    PlayerInfo.PlayerAim = Recv["aim"];
                    passData[9] = PlayerInfo.PlayerAim;
                }
            }

            if (Recv.ContainsKey("pic"))
            {
                int pic = 0;

                if (int.TryParse(Recv["pic"], out pic))
                {
                    if (pic != PlayerInfo.PlayerPicture)
                    {
                        query += ", picture=@P10";
                        PlayerInfo.PlayerPicture = pic;
                        passData[10] = pic;
                    }
                }
            }

            if (Recv.ContainsKey("occ"))
            {
                int occ = 0;

                if (int.TryParse(Recv["occ"], out occ))
                {
                    if (occ != PlayerInfo.PlayerOccupation)
                    {
                        query += ", occupationid=@P11";
                        PlayerInfo.PlayerOccupation = occ;
                        passData[11] = occ;
                    }
                }
            }

            if (Recv.ContainsKey("ind"))
            {
                int ind = 0;

                if (int.TryParse(Recv["ind"], out ind))
                {
                    if (ind != PlayerInfo.PlayerIndustryID)
                    {
                        query += ", industryid=@P12";
                        PlayerInfo.PlayerIndustryID = ind;
                        passData[12] = ind;
                    }
                }
            }

            if (Recv.ContainsKey("inc"))
            {
                int inc = 0;

                if (int.TryParse(Recv["inc"], out inc))
                {
                    if (inc != PlayerInfo.PlayerIncomeID)
                    {
                        query += ", industryid=@P13";
                        PlayerInfo.PlayerIncomeID = inc;
                        passData[13] = inc;
                    }
                }
            }

            if (Recv.ContainsKey("mar"))
            {
                int mar = 0;

                if (int.TryParse(Recv["mar"], out mar))
                {
                    if (mar != PlayerInfo.PlayerMarried)
                    {
                        query += ", marriedid=@P14";
                        PlayerInfo.PlayerMarried = mar;
                        passData[14] = mar;
                    }
                }
            }

            if (Recv.ContainsKey("chc"))
            {
                int chc = 0;

                if (int.TryParse(Recv["chc"], out chc))
                {
                    if (chc != PlayerInfo.PlayerChildCount)
                    {
                        query += ", childcount=@P15";
                        PlayerInfo.PlayerChildCount = chc;
                        passData[15] = chc;
                    }
                }
            }

            if (Recv.ContainsKey("i1"))
            {
                int i1 = 0;

                if (int.TryParse(Recv["i1"], out i1))
                {
                    if (i1 != PlayerInfo.PlayerInterests)
                    {
                        query += ", interests1=@P16";
                        PlayerInfo.PlayerInterests = i1;
                        passData[16] = i1;
                    }
                }
            }

            if (Recv.ContainsKey("nick"))
            {
                if (Recv["nick"] != PlayerInfo.PlayerNick)
                {
                    query += ", nick=@P17";
                    PlayerInfo.PlayerNick = Recv["nick"];
                    passData[17] = PlayerInfo.PlayerNick;
                }
            }

            if (Recv.ContainsKey("uniquenick"))
            {
                if (Recv["uniquenick"] != PlayerInfo.PlayerUniqueNick)
                {
                    query += ", uniquenick=@P18";
                    PlayerInfo.PlayerHomepage = Recv["uniquenick"];
                    passData[18] = PlayerInfo.PlayerUniqueNick;
                }
            }

            if (query == "UPDATE profiles SET")
                return;

            query = query.Replace("SET,", "SET");

            passData[21] = PlayerInfo.PlayerId;
            query += " WHERE `profileid`=@P21";

            try
            {
                //GPCMHelper.DBQuery.Query(query, passData);
                //i just wanna this program run for debugging
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
            if (PlayerInfo.LoginStatus == LoginStatus.Completed)
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
            StringBuilder HashString = new StringBuilder(PlayerInfo.PasswordHash);
            HashString.Append(' ', 48); // 48 spaces
            HashString.Append(realUserData);
            HashString.Append(challenge1);
            HashString.Append(challenge2);
            HashString.Append(PlayerInfo.PasswordHash);
            return HashString.ToString().GetMD5Hash();
        }
        #endregion

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
