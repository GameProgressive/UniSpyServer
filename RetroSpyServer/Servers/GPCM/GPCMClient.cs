using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using RetroSpyServer.Application;
using RetroSpyServer.Servers.GPCM.Enumerator;
using RetroSpyServer.Servers.GPCM.Structures;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

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
        public bool ProfileSent = false;

        /// <summary>
        /// This boolean checks if the client has received buddy information
        /// </summary>
        private bool BuddiesSent = false;

        /// <summary>
        /// The users session key
        /// </summary>
        public ushort SessionKey {get; protected set;}


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

            SessionKey = 0;

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
                LogWriter.Log.Write("The server challenge has already been sent. Cannot send another login challenge.",LogLevel.Warning);    

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
                Dictionary<string, string> dict = GamespyUtils.ConvertGPResponseToKeyValue(recieved);
                switch (recieved[0])
                {
                    case "inviteto":
                        GPCMHelper.AddProducts(this,dict);
                        break;
                    case "newuser":
                        GPCMHelper.CreateNewUser(this,dict);
                        break;
                    case "login":
                        ProcessLogin(dict);
                        break;
                    case "getprofile":
                        GPCMHelper.SendProfile(this,dict);
                        break;
                    case "updatepro":
                        GPCMHelper.UpdateUser(this, dict);
                        break;
                    case "logout":
                        Disconnect(DisconnectReason.NormalLogout);
                        break;
                    case "status":
                        UpdateStatus(dict);
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
        #endregion Steps

                    

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
