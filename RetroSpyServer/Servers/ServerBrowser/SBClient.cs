using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using RetroSpyServer.Application;
using RetroSpyServer.Servers.QueryReport.GameServerInfo;
using RetroSpyServer.Servers.QueryReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace RetroSpyServer.Servers.ServerBrowser
{
    public class SBClient
    {
        /// <summary>
        /// A unqie identifier for this connection
        /// </summary>
        public long ConnectionID;

        /// <summary>
        /// Indicates whether this object is disposed
        /// </summary>
        public bool Disposed { get; protected set; } = false;

        /// <summary>
        /// The clients socket network stream
        /// </summary>
        public TcpStream Stream { get; protected set; }

        /// <summary>
        /// Event fired when the connection is closed
        /// </summary>
        public static event SBConnectionClosed OnDisconnect;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        public SBClient(TcpStream stream, long connectionId)
        {
            // Generate a unique name for this connection
            ConnectionID = connectionId;
            // Init a new client stream class
            Stream = stream;
            Stream.OnDisconnect += Dispose;
            Stream.DataReceived += Stream_DataRecieved;
        }

            protected void Stream_DataRecieved(string received)
            {
                // lets split up the message based on the delimiter
                string[] messages = received.Split(new string[] { "\x00\x00\x00\x00" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string message in messages)
                {
                    // Ignore Non-BF2 related queries
                    if (message.StartsWith("battlefield2"))
                        ParseRequest(message);
                }            
        }


        /// <summary>
        /// Destructor
        /// </summary>
        ~SBClient()
        {
            if (!Disposed)
                Dispose(false);
        }

        public void Dispose()
        {
            if (!Disposed)
                Dispose(false);
        }

        /// <summary>
        /// Dispose method to be called by the server
        /// </summary>
        public void Dispose(bool DisposeEventArgs = false)
        {
            // Only dispose once
            if (Disposed) return;

            // Preapare to be unloaded from memory
            Disposed = true;

            // If connection is still alive, disconnect user
            if (!Stream.SocketClosed)
                Stream.Close(DisposeEventArgs);

            // Call disconnect event
            if (OnDisconnect != null)
                OnDisconnect(this);
        }

        /// <summary>
        /// Takes a message sent through the Stream and sends back a respose
        /// </summary>
        /// <param name="message"></param>
        protected void ParseRequest(string message)
        {
            string[] data = message.Split(new char[] { '\x00' }, StringSplitOptions.RemoveEmptyEntries);

            string gamename = data[1].ToLowerInvariant();
            string validate = data[2].Substring(0, 8);
            string filter = FixFilter(data[2].Substring(8));
            string[] fields = data[3].Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

            // Send the encrypted serverlist to the client
            byte[] unencryptedServerList = PackServerList(filter, fields);
            Stream.SendAsync(
                Enctypex.Encode(
                    Encoding.UTF8.GetBytes("hW6m9a"), // Battlfield 2 Handoff Key
                    Encoding.UTF8.GetBytes(validate),
                    unencryptedServerList,
                    unencryptedServerList.LongLength
                )
            );
        }

        /// <summary>
        /// Packs and prepares the response to a Server List request from the clients game.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        private byte[] PackServerList(string filter, string[] fields)
        {
            IPEndPoint remoteEndPoint = ((IPEndPoint)Stream.RemoteEndPoint);

            byte fieldsCount = (byte)fields.Length;
            byte[] ipBytes = remoteEndPoint.Address.GetAddressBytes();
            byte[] value2 = BitConverter.GetBytes((ushort)6500);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value2, 0, value2.Length);

            List<byte> data = new List<byte>();
            data.AddRange(ipBytes);
            data.AddRange(value2);
            data.Add(fieldsCount);
            data.Add(0);

            foreach (var field in fields)
            {
                data.AddRange(Encoding.UTF8.GetBytes(field));
                data.AddRange(new byte[] { 0, 0 });
            }

            // TODO: after implementing QueryReport, please touch this function again, this changes were done just to compile the server
            throw new NotImplementedException("SB after QR2");
            
            // Execute query right here in memory
            /*IQueryable<GameServer> servers = QueryReport.Servers.ToList().Select(x => x.Value).Where(x => x.IsValidated).AsQueryable();
            if (!String.IsNullOrWhiteSpace(filter))
            {
                try
                {
                    // Apply Filter
                    servers = servers.Where(filter);
                }
                catch (Exception e)
                {
                    LogWriter.Log.WriteException(e);
                    //Program.ErrorLog.Write("ERROR: [PackServerList] " + e.Message);
                    // Program.ErrorLog.Write(" - Filter Used: " + filter);
                }
            }


            // Add Servers
            foreach (GameServer server in servers)
            {
                // Get port bytes
                byte[] portBytes = BitConverter.GetBytes((ushort)server.QueryPort);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(portBytes, 0, portBytes.Length);

                data.Add(81); // it could be 85 as well, unsure of the difference, but 81 seems more common...
                data.AddRange(server.AddressInfo.Address.GetAddressBytes());
                data.AddRange(portBytes);
                data.Add(255);

                for (int i = 0; i < fields.Length; i++)
                {
                    data.AddRange(Encoding.UTF8.GetBytes(GetField(server, fields[i])));
                    if (i < fields.Length - 1)
                        data.AddRange(new byte[] { 0, 255 }); // Field Seperator
                }

                data.Add(0);
            }

            data.AddRange(new byte[] { 0, 255, 255, 255, 255 });
            return data.ToArray();*/
        }

        /// <summary>
        /// Fetches a property by fieldName from the provided Server Object
        /// </summary>
        /// <param name="server">The server we are fetching the field value from</param>
        /// <param name="fieldName">the field value we want</param>
        /// <returns></returns>
        private static string GetField(GameServer server, string fieldName)
        {
            object value = server.GetType().GetProperty(fieldName).GetValue(server, null);
            if (value == null)
                return String.Empty;
            else if (value is Boolean)
                return (bool)value ? "1" : "0";
            else
                return value.ToString();
        }

        /// <summary>
        /// A simple method of getting the value of the passed parameter key,
        /// from the returned array of data from the client
        /// </summary>
        /// <param name="parts">The array of data from the client</param>
        /// <param name="parameter">The parameter</param>
        /// <returns>The value of the paramenter key</returns>
        private string GetParameterValue(string[] parts, string parameter)
        {
            bool next = false;
            foreach (string part in parts)
            {
                if (next)
                    return part;
                else if (part == parameter)
                    next = true;
            }
            return "";
        }

        private string FixFilter(string filter)
        {
            // escape [
            filter = filter.Replace("[", "[[]");
            try
            {
                // fix an issue in the BF2 main menu where filter expressions aren't joined properly
                // i.e. "numplayers > 0gametype like '%gpm_cq%'"
                // becomes "numplayers > 0 && gametype like '%gpm_cq%'"
                filter = FixFilterOperators(filter);

                // fix quotes inside quotes
                // i.e. hostname like 'flyin' high'
                // becomes hostname like 'flyin_ high'
                filter = FixFilterQuotes(filter);
            }
            catch (Exception)
            {
                //LogError(Category, e.ToString());
            }

            // fix consecutive whitespace
            return Regex.Replace(filter, @"\s+", " ").Trim();
        }

        private static string FixFilterOperators(string filter)
        {
            PropertyInfo[] properties = typeof(GameServer).GetProperties();
            List<string> filterableProperties = new List<string>();

            // get all the properties that aren't "[NonFilter]"
            foreach (var property in properties)
            {
                if (property.GetCustomAttributes(false).Any(x => x.GetType().Name == "NonFilterAttribute"))
                    continue;

                filterableProperties.Add(property.Name);
            }

            // go through each property, see if they exist in the filter,
            // and check to see if what's before the property is a logical operator
            // if it is not, then we slap a && before it
            foreach (var property in filterableProperties)
            {
                IEnumerable<int> indexes = filter.IndexesOf(property);
                foreach (var index in indexes)
                {
                    if (index > 0)
                    {
                        int length = 0;
                        bool hasLogical = IsLogical(filter, index, out length, true) || IsOperator(filter, index, out length, true) || IsGroup(filter, index, out length, true);
                        if (!hasLogical)
                        {
                            filter = filter.Insert(index, " && ");
                        }
                    }
                }
            }
            return filter;
        }

        private static string FixFilterQuotes(string filter)
        {
            StringBuilder newFilter = new StringBuilder(filter);

            for (int i = 0; i < filter.Length; i++)
            {
                int length = 0;
                bool isOperator = IsOperator(filter, i, out length);

                if (isOperator)
                {
                    i += length;
                    bool isInsideString = false;
                    for (; i < filter.Length; i++)
                    {
                        if (filter[i] == '\'' || filter[i] == '"')
                        {
                            if (isInsideString)
                            {
                                // check what's after the quote to see if we terminate the string
                                if (i >= filter.Length - 1)
                                {
                                    // end of string
                                    isInsideString = false;
                                    break;
                                }
                                for (int j = i + 1; j < filter.Length; j++)
                                {
                                    // continue along whitespace
                                    if (filter[j] == ' ')
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        // if it's a logical operator, then we terminate
                                        bool op = IsLogical(filter, j, out length);
                                        if (op)
                                        {
                                            isInsideString = false;
                                            j += length;
                                            i = j;
                                        }
                                        break;
                                    }
                                }
                                if (isInsideString)
                                {
                                    // and if we're still inside the string, replace the quote with a wildcard character
                                    newFilter[i] = '_';
                                }
                                continue;
                            }
                            else
                            {
                                isInsideString = true;
                            }
                        }
                    }
                }
            }

            return newFilter.ToString();
        }

        private static bool IsOperator(string filter, int i, out int length, bool previous = false)
        {
            bool isOperator = false;
            length = 0;

            if (i < filter.Length - 1)
            {
                string op = filter.Substring(i - (i >= 2 ? (previous ? 2 : 0) : 0), 1);
                if (op == "=" || op == "<" || op == ">")
                {
                    isOperator = true;
                    length = 1;
                }
            }

            if (!isOperator)
            {
                if (i < filter.Length - 2)
                {
                    string op = filter.Substring(i - (i >= 3 ? (previous ? 3 : 0) : 0), 2);
                    if (op == "==" || op == "!=" || op == "<>" || op == "<=" || op == ">=")
                    {
                        isOperator = true;
                        length = 2;
                    }
                }
            }

            if (!isOperator)
            {
                if (i < filter.Length - 4)
                {
                    string op = filter.Substring(i - (i >= 5 ? (previous ? 5 : 0) : 0), 4);
                    if (op.Equals("like", StringComparison.InvariantCultureIgnoreCase))
                    {
                        isOperator = true;
                        length = 4;
                    }
                }
            }

            if (!isOperator)
            {
                if (i < filter.Length - 8)
                {
                    string op = filter.Substring(i - (i >= 9 ? (previous ? 9 : 0) : 0), 8);
                    if (op.Equals("not like", StringComparison.InvariantCultureIgnoreCase))
                    {
                        isOperator = true;
                        length = 8;
                    }
                }
            }

            return isOperator;
        }

        private static bool IsLogical(string filter, int i, out int length, bool previous = false)
        {
            bool isLogical = false;
            length = 0;

            if (i < filter.Length - 2)
            {
                string op = filter.Substring(i - (i >= 3 ? (previous ? 3 : 0) : 0), 2);
                if (op == "&&" || op == "||" || op.Equals("or", StringComparison.InvariantCultureIgnoreCase))
                {
                    isLogical = true;
                    length = 2;
                }
            }

            if (!isLogical)
            {
                if (i < filter.Length - 3)
                {
                    string op = filter.Substring(i - (i >= 4 ? (previous ? 4 : 0) : 0), 3);
                    if (op.Equals("and", StringComparison.InvariantCultureIgnoreCase))
                    {
                        isLogical = true;
                        length = 3;
                    }
                }
            }

            return isLogical;
        }

        private static bool IsGroup(string filter, int i, out int length, bool previous = false)
        {
            bool isGroup = false;
            length = 0;

            if (i < filter.Length - 1)
            {
                string op = filter.Substring(i - (i >= 2 ? (previous ? 2 : 0) : 0), 1);
                if (op == "(" || op == ")")
                {
                    isGroup = true;
                    length = 1;
                }
                if (!isGroup && previous)
                {
                    op = filter.Substring(i - (i >= 1 ? (previous ? 1 : 0) : 0), 1);
                    if (op == "(") // || op == ")")
                    {
                        isGroup = true;
                        length = 1;
                    }
                }
            }

            return isGroup;
        }
    }
}
