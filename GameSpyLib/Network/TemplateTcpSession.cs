using System;
using System.Net.Sockets;
using System.Text;
using NetCoreServer;
using GameSpyLib.Logging;

namespace GameSpyLib.Network
{
    /// <summary>
    /// This is a template class that helps creating a TCP Session (formerly TCP stream) with logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public class TemplateTcpSession : TcpSession
    {
        /// <summary>
        /// The name of the server that is started, used primary in logging functions.
        /// </summary>
        protected string ServerName = "";

        /// <summary>
        /// Some server requires that clients should disconnect after send, we use this to determine whether disconnecting clients
        /// </summary>
        protected bool DisconnectAfterSend = false;

        public TemplateTcpSession(TemplateTcpServer server) : base(server)
        {
            ServerName = server.ServerName;
        }

        /// <summary>
        /// Handle error notification
        /// </summary>
        /// <param name="error">Socket error code</param>
        protected override void OnError(SocketError error)
        {
            LogWriter.Log.Write(LogLevel.Error, "[{0}] Error: {1}", ServerName, Enum.GetName(typeof(SocketError), error));
        }

        /// <summary>
        /// Send data to the client (asynchronous)
        /// </summary>
        /// <param name="buffer">Buffer to send</param>
        /// <param name="offset">Buffer offset</param>
        /// <param name="size">Buffer size</param>
        /// <returns>'true' if the data was successfully sent, 'false' if the session is not connected</returns>
        /// <remarks>
        /// We override this method in order to let it print the data it transmits, please call "base.SendAsync" in your overrided function.
        /// </remarks>
        public override bool SendAsync(byte[] buffer, long offset, long size)
        {
            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write(LogLevel.Debug, "[{0}] [Send] TCP data: {1}", ServerName, Encoding.UTF8.GetString(buffer));

            bool returnValue = base.SendAsync(buffer, offset, size);

            if (DisconnectAfterSend)
                Disconnect();

            return returnValue;
        }

        /// <summary>
        /// Send data to the client (synchronous)
        /// </summary>
        /// <param name="buffer">Buffer to send</param>
        /// <param name="offset">Buffer offset</param>
        /// <param name="size">Buffer size</param>
        /// <returns>Size of sent data</returns>
        /// <remarks>
        /// We override this method in order to let it print the data it transmits, please call "base.Send" in your overrided function.
        /// </remarks>
        public override long Send(byte[] buffer, long offset, long size)
        {
            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write(LogLevel.Debug, "[{0}] [Send] TCP data: {1}", ServerName, Encoding.UTF8.GetString(buffer));

            long returnValue = base.Send(buffer, offset, size);

            if (DisconnectAfterSend)
                Disconnect();

            return returnValue;
        }

        /// <summary>
        /// Handle buffer received notification
        /// </summary>
        /// <param name="buffer">Received buffer</param>
        /// <param name="offset">Received buffer offset</param>
        /// <param name="size">Received buffer size</param>
        /// <remarks>
        /// Notification is called when another chunk of buffer was received from the client
        /// We override this method in order to let it print the data it transmits, please call "base.OnReceived" in your overrided function
        /// </remarks>
        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write(LogLevel.Debug, "[{0}] [Recv] TCP data: {1}", ServerName, Encoding.UTF8.GetString(buffer, 0, (int)size));
        }
    }
}
