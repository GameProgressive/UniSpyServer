using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using GameSpyLib.Gamespy.Net;
using GameSpyLib.Gamespy;
using GameSpyLib.Logging;

namespace RetroSpyServer
{
    public class GPSPServer : GamespyTcpSocket
    {
        public GPSPServer(IPEndPoint bindTo, int MaxConnections) : base(bindTo, MaxConnections)
        {
            // Start accepting clients
            StartAcceptAsync();
        }

        /// <summary>
        /// Send a GPSP error
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="code"></param>
        /// <param name="error"></param>
        protected void SendError(Socket socket, int code, string error)
        {
            socket.Send(Encoding.UTF8.GetBytes(String.Format(@"\error\\err\{0}\fatal\\errmsg\{1}\id\1\final\", code, error)));
        }

        /// <summary>
        /// This function is fired when the server is unable to accept the client
        /// </summary>
        /// <param name="socket"></param>
        protected override void OnAcceptFails(Socket socket)
        {
            SendError(socket, 0, FullErrorMessage);
        }

        /// <summary>
        /// This function is fired when an exception occour in the server
        /// </summary>
        /// <param name="e">The exception to be thrown</param>
        protected override void OnException(Exception e)
        {
            throw e;
        }

        /// <summary>
        /// Converts a recived parameter array from the client string to a keyValue pair dictionary
        /// </summary>
        /// <param name="parts">The array of data from the client</param>
        /// <returns></returns>
        private static Dictionary<string, string> ConvertToKeyValue(string[] parts)
        {
            Dictionary<string, string> Data = new Dictionary<string, string>();
            try
            {
                for (int i = 0; i < parts.Length; i += 2)
                {
                    if (!Data.ContainsKey(parts[i]))
                        Data.Add(parts[i], parts[i + 1]);
                }
            }
            catch (IndexOutOfRangeException) { }

            return Data;
        }

        /// <summary>
        /// This function is fired when a client is being accepted
        /// </summary>
        /// <param name="Stream">The stream of the client to be accepted</param>
        protected override void ProcessAccept(GamespyTcpStream Stream)
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
        protected void ProcessDataReceived(GamespyTcpStream stream, string message)
        {
            string[] received = message.TrimStart('\\').Split('\\');
            Dictionary<string, string> dict = ConvertToKeyValue(received);

            switch (received[0])
            {
                case "vaild":
                    IsEmailValid(stream, dict);
                    break;
                case "nicks":
                    OnNicks(stream, dict);
                    break;
                case "check":
                    OnCheck(stream, dict);
                    break;
                case "search":
                    OnSearch(stream, dict);
                    break;
                case "others":
                    OnOthers(stream, dict);
                    break;
                case "otherslist":
                    OnOthersList(stream, dict);
                    break;
                case "uniquesearch":
                    OnUniqueSearch(stream, dict);
                    break;
                case "profilelist":
                    OnProfileList(stream, dict);
                    break;
                case "pmatch":
                    OnProoductMatch(stream, dict);
                    break;
                case "newuser":
                    OnNewUser(stream, dict);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void OnUniqueSearch(GamespyTcpStream stream, Dictionary<string, string> dict)
        {
            throw new NotImplementedException();
        }

        private void OnProfileList(GamespyTcpStream stream, Dictionary<string, string> dict)
        {
            throw new NotImplementedException();
        }

        private void OnProoductMatch(GamespyTcpStream stream, Dictionary<string, string> dict)
        {
            throw new NotImplementedException();
        }

        private void OnNewUser(GamespyTcpStream stream, Dictionary<string, string> dict)
        {
            throw new NotImplementedException();
        }

        private void OnOthersList(GamespyTcpStream stream, Dictionary<string, string> dict)
        {
            throw new NotImplementedException();
        }

        private void OnOthers(GamespyTcpStream stream, Dictionary<string, string> dict)
        {
            throw new NotImplementedException();
        }

        private void OnSearch(GamespyTcpStream stream, Dictionary<string, string> dict)
        {
            throw new NotImplementedException();
        }

        private void OnCheck(GamespyTcpStream stream, Dictionary<string, string> dict)
        {
            throw new NotImplementedException();
        }

        private void OnNicks(GamespyTcpStream stream, Dictionary<string, string> dict)
        {
            throw new NotImplementedException();
        }

        private void IsEmailValid(GamespyTcpStream stream, Dictionary<string, string> dict)
        {
            throw new NotImplementedException();
        }
    }
}
