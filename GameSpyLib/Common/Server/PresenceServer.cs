using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using GameSpyLib.Database;
using GameSpyLib.Network;
using GameSpyLib.Logging;

namespace GameSpyLib.Server
{
    public abstract class PresenceServer : TCPServer
    {
        /// <summary>
        /// Send a presence error
        /// </summary>
        /// <param name="stream">The stream that will receive the error</param>
        /// <param name="code">The error code</param>
        /// <param name="error">A string containing the error</param>
        protected void SendError(TCPStream stream, int code, string error)
        {
            stream.SendAsync(Encoding.UTF8.GetBytes(String.Format(@"\error\\err\{0}\fatal\\errmsg\{1}\id\1\final\", code, error)));
        }

        /// <summary>
        /// Send a presence error
        /// </summary>
        /// <param name="socket">The socket that will receive the error</param>
        /// <param name="code">The error code</param>
        /// <param name="error">A string containing the error</param>
        protected void SendError(Socket socket, int code, string error)
        {
            socket.Send(Encoding.UTF8.GetBytes(String.Format(@"\error\\err\{0}\fatal\\errmsg\{1}\id\1\final\", code, error)));
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="databaseDriver">
        /// A connection to a database
        /// NOTE: The connection must not be null
        /// </param>
        public PresenceServer(DatabaseDriver databaseDriver) : base(databaseDriver)
        {
            if (databaseDriver == null)
                throw new NullReferenceException("databaseDriver cannot be null!");
        }

        /// <summary>
        /// If the server fails to accept a client, this function is fired
        /// </summary>
        /// <param name="socket">The socket that failed to be accepted</param>
        protected override void OnAcceptFails(Socket socket)
        {
            SendError(socket, 0, FullErrorMessage);
        }

        protected void PrintReceivedDictToLogger(string req, Dictionary<string, string> dict)
        {
            LogWriter.Debug( String.Format("Received request {0} with content: {1}", req, String.Join(";", dict.Select(x => x.Key + "=" + x.Value).ToArray()) ) );
        }

        /// <summary>
        /// Converts a recived parameter array from the client string to a keyValue pair dictionary
        /// </summary>
        /// <param name="parts">The array of data from the client</param>
        /// <returns></returns>
        protected Dictionary<string, string> ConvertToKeyValue(string[] parts)
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
    }
}
