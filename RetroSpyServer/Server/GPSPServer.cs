using System;
using System.Collections.Generic;
using GameSpyLib.Database;
using GameSpyLib.Server;
using GameSpyLib.Network;

namespace RetroSpyServer
{
    public class GPSPServer : PresenceServer
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="databaseDriver">
        /// A connection to a database
        /// NOTE: The connection must not be null
        /// </param>
        public GPSPServer(DatabaseDriver databaseDriver) : base(databaseDriver)
        {
        }

        enum RequestsCheck
        {
            valid,
            nicks,
            check,
            search,
            others,
            otherslist,
            uniquesearch,
            profilelist,
            pmatch,
            newuser
        };

        /// <summary>
        /// This function is fired when an exception occour in the server
        /// </summary>
        /// <param name="e">The exception to be thrown</param>
        protected override void OnException(Exception e)
        {
            throw e;
        }

        /// <summary>
        /// This function is fired when a client is being accepted
        /// </summary>
        /// <param name="Stream">The stream of the client to be accepted</param>
        protected override void ProcessAccept(TCPStream Stream)
        {
            Stream.DataReceived += ProcessDataReceived;
            Stream.OnDisconnect += (stream) => stream.Dispose();
            Stream.BeginReceive();
        }

        /// <summary>
        /// This function is fired when data is received from a stream
        /// </summary>
        /// <param name="stream">The stream that sended the data</param>
        /// <param name="message">The message the stream sended</param>
        protected void ProcessDataReceived(TCPStream stream, string message)
        {
            string[] received = message.TrimStart('\\').Split('\\');
            Dictionary<string, string> dict = ConvertToKeyValue(received);
            Enum.TryParse(received[0], out RequestsCheck request);

            switch (request)
            {
                case RequestsCheck.valid:
                     IsEmailValid(stream, dict);
                     break;
                case RequestsCheck.nicks:
                     RetriveNicknames(stream, dict);
                     break;
                case RequestsCheck.check:
                     CheckAccount(stream, dict);
                     break;
                case RequestsCheck.search:
                     SearchUser(stream, dict);
                     break;
                case RequestsCheck.others:
                     ReverseBuddies(stream, dict);
                     break;
                case RequestsCheck.otherslist:
                     OnOthersList(stream, dict);
                     break;
                case RequestsCheck.uniquesearch:
                     SuggestUniqueNickname(stream, dict);
                     break;
                case RequestsCheck.profilelist:
                     OnProfileList(stream, dict);
                     break;
                case RequestsCheck.pmatch:
                     MatchProduct(stream, dict);
                     break;
                case RequestsCheck.newuser:
                     CreateUser(stream, dict);
                     break;
                default:
                    SendError(stream, 0, "An invalid request was sended.");
                    stream.Close(false);
                    break;
            }
        }

        private void SuggestUniqueNickname(TCPStream stream, Dictionary<string, string> dict)
        {
            SendError(stream, 0, "This request is not supported yet.");
            stream.Close(false);
        }

        private void OnProfileList(TCPStream stream, Dictionary<string, string> dict)
        {
            SendError(stream, 0, "This request is not supported yet.");
            stream.Close(false);
        }

        private void MatchProduct(TCPStream stream, Dictionary<string, string> dict)
        {
            SendError(stream, 0, "This request is not supported yet.");
            stream.Close(false);
        }

        /// <summary>
        /// Creates an account
        /// </summary>
        /// <param name="stream">The stream that sended the data</param>
        /// <param name="dict">The request that the stream sended</param>
        private void CreateUser(TCPStream stream, Dictionary<string, string> dict)
        {
            SendError(stream, 0, "This request is not supported yet.");
            stream.Close(false);

        }

        private void OnOthersList(TCPStream stream, Dictionary<string, string> dict)
        {
            SendError(stream, 0, "This request is not supported yet.");
            stream.Close(false);
        }

        private void ReverseBuddies(TCPStream stream, Dictionary<string, string> dict)
        {
            SendError(stream, 0, "This request is not supported yet.");
            stream.Close(false);

        }

        private void SearchUser(TCPStream stream, Dictionary<string, string> dict)
        {
            SendError(stream, 0, "This request is not supported yet.");
            stream.Close(false);
        }

        private void CheckAccount(TCPStream stream, Dictionary<string, string> dict)
        {
            SendError(stream, 0, "This request is not supported yet.");
            stream.Close(false);
        }

        private void RetriveNicknames(TCPStream stream, Dictionary<string, string> dict)
        {
            SendError(stream, 0, "This request is not supported yet.");
            stream.Close(false);
        }

        /// <summary>
        /// Checks if a provided email is valid
        /// </summary>
        /// <param name="stream">The stream that sended the data</param>
        /// <param name="dict">The request that the stream sended</param>
        private void IsEmailValid(TCPStream stream, Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey("email"))
            {
                SendError(stream, 1, "There was an error parsing an incoming request.");
                stream.Close();
            }

            try
            {
                if (databaseDriver.Query("SELECT id FROM users WHERE LOWER(email)=@P0", dict["email"].ToLowerInvariant()).Count != 0)
                    stream.SendAsync(@"\vr\1\final\");
                else
                    stream.SendAsync(@"\vr\0\final\");
            }
            catch (Exception)
            {
                SendError(stream, 4, "This request cannot be processed because of a database error.");
                stream.Close();
            }
        }
    }
}
