using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using GameSpyLib;
using GameSpyLib.Network;
using GameSpyLib.Logging;
using GameSpyLib.Server;

namespace RetroSpyServer.Server
{
    public enum DisconnectReason
    {
        Disconnected,
        ForcedServerShutdown,
        NormalLogout,
        ClientChallengeAlreadySent,
    };

    public enum LoginStatus
    {
        Disconnected,
        Working,
        Completed,
    }

    /// <summary>
    /// This class rapresents a client for GPCM Server
    /// </summary>
    public class GPCMClient : IDisposable, IEquatable<GPCMClient>
    {
        public long ConnectionID { get; protected set; }

        public TCPStream Stream { get; protected set; }

        public uint ProfileID { get; protected set; }

        public uint UserID { get; protected set; }

        public string Nickname { get; protected set; }

        public string UniqueNickname { get; protected set; }

        private ushort SessionKey;

        public bool IsDisposed { get; protected set; }

        public LoginStatus Status { get; protected set; }

        public IPEndPoint RemoteEndPoint { get; protected set; }


        /// <summary>
        /// Event fired when that remote connection logs out, or
        /// the socket gets disconnected. This event will not fire
        /// unless OnSuccessfulLogin event was fired first.
        /// </summary>
        public static event GpcmConnectionClosed OnDisconnect;

        public void Dispose()
        {
            IsDisposed = true;
        }
        
        public GPCMClient(TCPStream stream, long connectionId)
        {
            ConnectionID = connectionId;
            Stream = stream;
            ProfileID = 0;
            UserID = 0;
            SessionKey = 0;
            Nickname = "";
            UniqueNickname = "";

            RemoteEndPoint = (IPEndPoint)stream.RemoteEndPoint;

            // Set callbacks
            Stream.DataReceived += ProcessDataReceived;
            Stream.OnDisconnect += OnStreamDisconnected;
            Stream.BeginReceive();

            Status = LoginStatus.Working;
            
            IsDisposed = false;
        }

        /// <summary>
        /// Decostructor
        /// </summary>
        ~GPCMClient()
        {
            if (!IsDisposed)
                Dispose();
        }

        /// <summary>
        /// Disconnected the client
        /// </summary>
        /// <param name="reason">The reason to disconnect</param>
        public void Disconnect(DisconnectReason reason)
        {
            Stream.OnDisconnect -= OnStreamDisconnected;
            Stream.DataReceived -= ProcessDataReceived;
            Stream.Close(reason == DisconnectReason.ForcedServerShutdown);

            if (Status == LoginStatus.Completed)
            {
                if (reason == DisconnectReason.NormalLogout)
                    LogWriter.Log.Write("Client logout: {0}/{1} - {2}@{3} - {4}", LogLevel.Information, Nickname, UniqueNickname, ProfileID, UserID, RemoteEndPoint);
                else if (reason != DisconnectReason.ForcedServerShutdown)
                    LogWriter.Log.Write("Client disconnect: {0}/{1} - {2}@{3} - {4} Reason: {5}", LogLevel.Information, Nickname, UniqueNickname, ProfileID, UserID, RemoteEndPoint, Enum.GetName(typeof(DisconnectReason), reason));
            }

            Status = LoginStatus.Disconnected;
            Dispose();

            OnDisconnect?.Invoke(this);
        }

        protected void OnStreamDisconnected(TCPStream stream)
        {
            Disconnect(DisconnectReason.Disconnected);
        }

        /// <summary>
        /// This function is fired some data is received from the client
        /// </summary>
        /// <param name="stream">The stream that sended the data</param>
        /// <param name="message">The message that was received</param>
        public void ProcessDataReceived(TCPStream stream, string message)
        {
            if (stream != Stream)
                return; // What?

            string[] received = message.TrimStart('\\').Split('\\');

            switch (received[0])
            {
                case "newuser":
                    CreateNewUser(PresenceServer.ConvertToKeyValue(received));
                    break;
                case "login":
                    ProcessLogin(PresenceServer.ConvertToKeyValue(received));
                    break;
                case "getprofile":
                    SendProfileInformation();
                    break;
                case "updatepro":
                    UpdateUser(PresenceServer.ConvertToKeyValue(received));
                    break;
                case "logout":
                    Disconnect(DisconnectReason.NormalLogout);
                    break;
                default:
                    LogWriter.Log.Write("Received unknown request " + received[0], LogLevel.Debug);
                    PresenceServer.SendError(Stream, 0, "An invalid request was sended.");
                    break;
            }
        }

        private void UpdateUser(Dictionary<string, string> dictionary)
        {
            throw new NotImplementedException();
        }

        private void SendProfileInformation()
        {
            throw new NotImplementedException();
        }

        private void ProcessLogin(Dictionary<string, string> dictionary)
        {
            throw new NotImplementedException();
        }

        private void CreateNewUser(Dictionary<string, string> dictionary)
        {
            throw new NotImplementedException();
        }

        public void SendServerChallenge(uint serverID)
        {
            if (Status != LoginStatus.Completed)
            {
                Disconnect(DisconnectReason.ClientChallengeAlreadySent);
                return;
            }

            Status = LoginStatus.Working;
            Stream.SendAsync(@"\lc\1\challenge\{0}\id\{1}\final\", GameSpyLib.Random.GenerateRandomString(10, GameSpyLib.Random.StringType.Alpha), serverID);
        }


        public bool Equals(GPCMClient other)
        {
            if (other == null) return false;
            return (UserID == other.UserID || Nickname == other.Nickname);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as GPCMClient);
        }

        public override int GetHashCode()
        {
            return (int)UserID;
        }
    }
}
