using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using GameSpyLib.Common;
using GameSpyLib.Logging;

namespace GameSpyLib.Network
{
    /// <summary>
    /// This object is used as a Network Stream wrapper for Gamespy TCP protocol.
    /// </summary>
    public class TCPStream : IDisposable
    {
        /// <summary>
        ///The length of message that we can revieved.
        /// </summary>
        protected StringBuilder RecvMessage = new StringBuilder(4096);

        /// <summary>
        /// Our message to send to the client. If the message is too long, it will be sent
        /// over multiple write operations, so we store the message here until its all sent
        /// </summary>
        protected List<byte> SendMessage = new List<byte>(256);

        /// <summary>
        /// The current send offset when sending asynchronously
        /// </summary>
        protected int SendBytesOffset = 0;

        /// <summary>
        /// Indicates whether we are currently sending a message asynchronously
        /// </summary>
        protected bool WaitingOnAsync = false;

        /// <summary>
        /// Our connected socket
        /// </summary>
        public Socket Connection;

        protected int DataAttempt = 0;

        /// <summary>
        /// Contains the GamespyTcpSocket that owns this object
        /// </summary>
        public TCPServer SocketManager { get; protected set; }

        /// <summary>
        /// Our AsycnEventArgs object for reading data
        /// </summary>
        public SocketAsyncEventArgs ReadEventArgs { get; protected set; }

        /// <summary>
        /// Our AsyncEventArgs object for sending data
        /// </summary>
        public SocketAsyncEventArgs WriteEventArgs { get; protected set; }

        /// <summary>
        /// Gets the remote endpoint
        /// </summary>
        public EndPoint RemoteEndPoint
        {
            get { return ReadEventArgs.AcceptSocket.RemoteEndPoint; }
        }

        /// <summary>
        /// Indicates whether the underlying socket connection has been closed,
        /// and cleaned up properly
        /// </summary>
        public bool SocketClosed { get; protected set; }

        /// <summary>
        /// Indicates whether this stream has been released to the SocketManager
        /// </summary>
        public bool Released { get; protected set; }

        /// <summary>
        /// Indicates whether the OnDisconnect event has been called
        /// </summary>
        protected bool DisconnectEventCalled = false;

        /// <summary>
        /// Indicates whether the EventArgs objects were disposed by request
        /// <remarks>This should NEVER be true unless we are shutting down the server!!!</remarks>
        /// </summary>
        public bool DisposedEventArgs { get; protected set; }

        /// <summary>
        /// Event fired when a completed message has been received
        /// </summary>
        public event DataRecivedEvent OnDataReceived;

        /// <summary>
        /// Event fired when the remote connection is closed
        /// </summary>
        public event ConnectionClosed OnDisconnected;

        /// <summary>
        /// Event fired to check if the message is finished or not (required for gp* servers)
        /// </summary>
        public event MessageFinished IsMessageFinished;

        /// <summary>
        /// An object to lock onto
        /// </summary>
        private object _lockObj = new object();

        /// <summary>
        /// Creates a new instance of GamespyTcpStream
        /// </summary>
        /// <param name="ReadArgs"></param>
        public TCPStream(TCPServer Parent, SocketAsyncEventArgs ReadArgs, SocketAsyncEventArgs WritetArgs)
        {            
            // Store our connection
            Connection = ReadArgs.AcceptSocket;
            SocketManager = Parent;

            // Create our IO event callbacks
            ReadArgs.Completed += IOComplete;
            WritetArgs.Completed += IOComplete;

            // Set our internal variables
            ReadEventArgs = ReadArgs;
            WriteEventArgs = WritetArgs;
            SocketClosed = false;
            DisposedEventArgs = false;
            Released = false;
        }

        ~TCPStream()
        {
            if (!SocketClosed)
                Close();
        }

        public void Dispose()
        {
            if (!SocketClosed)
                Close();
        }

        /// <summary>
        /// Begins the process of receiving a message from the client.
        /// This method must manually be called to Begin receiving data
        /// </summary>
        public void BeginReceive()
        {
            try
            {
                if (Connection != null)
                {
                    // Reset Buffer offset back to the original allocated offset
                    BufferDataToken token = ReadEventArgs.UserToken as BufferDataToken;
                    ReadEventArgs.SetBuffer(token.BufferOffset, token.BufferBlockSize);

                    // Begin Receiving
                    if (!Connection.ReceiveAsync(ReadEventArgs))
                        ProcessReceive();
                }
            }
            catch (ObjectDisposedException)
            {
                if (!DisconnectEventCalled)
                {
                    // Disconnect user
                    DisconnectEventCalled = true;
                    OnDisconnected?.Invoke();
                }
            }
            catch (SocketException e)
            {
                HandleSocketError(e.SocketErrorCode);
            }
        }

        /// <summary>
        /// Closes the underlying socket
        /// </summary>
        /// <param name="DisposeEventArgs">
        /// If true, the EventArg objects will be disposed instead of being re-added to 
        /// the IO pool. This should NEVER be set to true unless we are shutting down the server!
        /// </param>
        public void Close(bool DisposeEventArgs = false)
        {
            // Set that the socket is being closed once, and properly
            if (SocketClosed) return;
            SocketClosed = true;

            // Do a shutdown before you close the socket
            try
            {
                Connection.Shutdown(SocketShutdown.Both);
            }
            catch (Exception) { }
            finally
            {
                // Unregister for vents
                ReadEventArgs.Completed -= IOComplete;
                WriteEventArgs.Completed -= IOComplete;

                // Close the connection
                Connection.Close();
                Connection = null;
            }

            // If we need to dispose out EventArgs
            if (DisposeEventArgs)
            {
                ReadEventArgs.Dispose();
                WriteEventArgs.Dispose();
                DisposedEventArgs = true;
            }
            else
            {
                // Finally, release this stream so we can allow a new connection
                SocketManager.Release(this);
                Released = true;
            }

            // Call Disconnect Event
            if (!DisconnectEventCalled && OnDisconnected != null)
            {
                DisconnectEventCalled = true;
                OnDisconnected();
            }
        }

        /// <summary>
        /// Once data has been recived from the client, this method is called
        /// to process the data. Once a message has been completed, the OnDataReceived
        /// event will be called
        /// </summary>
        private void ProcessReceive()
        {
            // If we do not get a success code here, we have a bad socket
            if (ReadEventArgs.SocketError != SocketError.Success)
            {
                HandleSocketError(ReadEventArgs.SocketError);
                return;
            }

            // Force disconnect (Specifically for Gpsp, whom will spam empty connections)
            if (ReadEventArgs.BytesTransferred == 0)
            {
                Close();
                return;
            }
            else
            {
                // Fetch our message as a string from the Buffer
                BufferDataToken token = ReadEventArgs.UserToken as BufferDataToken;
                RecvMessage.Append(
                    Encoding.UTF8.GetString(
                        ReadEventArgs.Buffer,
                        token.BufferOffset,
                        ReadEventArgs.BytesTransferred
                    )
                );

                // Process Message
                string received = RecvMessage.ToString();

                if (DataAttempt < 5)
                {
                    if (IsMessageFinished.Invoke(received))
                    {
                        if (LogWriter.Log.DebugSockets)
                            LogWriter.Log.Write(LogLevel.Debug, "{0} [Recv] TCP data: {1}" ,SocketManager.ServerName, received);

                        DataAttempt = 0;

                        // tell our parent that we recieved a message
                        RecvMessage.Clear(); // Clear old junk
                        OnDataReceived.Invoke(received);
                    }

                    DataAttempt++;
                }
                else
                {
                    // Looks like the client is sending a lot of data that is not valid
                    LogWriter.Log.Write(LogLevel.Info, "TCP stream {0} is sending a lot of data! Connection closed.",  RemoteEndPoint);
                    Close(false);
                }
            }

            // Begin receiving again
            BeginReceive();
        }

        /// <summary>
        /// Writes a message to the client stream asynchronously
        /// </summary>
        /// <param name="message">The complete message to be sent to the client</param>
        public void SendAsync(string message)
        {
            // Make sure the socket is still open
            if (SocketClosed) return;

            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write( LogLevel.Debug, "{0} [Send] TCP data: {1}" , SocketManager.ServerName, message);

            // Create a lock, so we don't add a message while the old one is being cleared
            lock (_lockObj)
                SendMessage.AddRange(Encoding.UTF8.GetBytes(message));

            // Send if we aren't already in the middle of an Async send
            if (!WaitingOnAsync) ProcessSend();
        }

        /// <summary>
        /// Writes a message to the client stream asynchronously
        /// </summary>
        /// <param name="message">The complete message to be sent to the client</param>
        /// <param name="items"></param>
        public void SendAsync(string message, params object[] items)
        {
            SendAsync(string.Format(message, items));
        }

        /// <summary>
        /// Writes a message to the client stream asynchronously
        /// </summary>
        /// <param name="message">The complete message to be sent to the client</param>
        public void SendAsync(byte[] message)
        {
            // Make sure the socket is still open
            if (SocketClosed) return;

            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write( LogLevel.Debug, "{0} [Send] TCP data: {1}", SocketManager.ServerName, Encoding.UTF8.GetString(message));

            // Create a lock, so we don't add a message while the old one is being cleared
            lock (_lockObj)
                SendMessage.AddRange(message);

            // Send if we aren't already in the middle of an Async send
            if (!WaitingOnAsync) ProcessSend();
        }

        /// <summary>
        /// Sends a message Asynchronously to the client connection
        /// </summary>
        private void ProcessSend()
        {
            // Return if we are closing the socket
            if (SocketClosed) return;

            // Bool holder
            bool willRaiseEvent = true;

            // Prevent an connection loss exception
            try
            {
                // Prevent race conditions by locking here.
                // ** Make sure to set WaitingOnAsync Inside the LOCK! **
                lock (_lockObj)
                {
                    // If we are waiting on the IO operation to complete, we exit here
                    if (WaitingOnAsync) return;

                    // Get the number of bytes remaining to be sent
                    int NumBytesToSend = SendMessage.Count - SendBytesOffset;

                    // If there are no more bytes to send, then reset
                    if (NumBytesToSend <= 0)
                    {
                        SendMessage.Clear();
                        SendBytesOffset = 0;
                        WaitingOnAsync = false;
                        return;
                    }

                    // Make sure we arent sending more data then what we have space for
                    BufferDataToken Token = WriteEventArgs.UserToken as BufferDataToken;
                    if (NumBytesToSend > Token.BufferBlockSize)
                        NumBytesToSend = Token.BufferBlockSize;

                    // Copy our message to the Write Buffer
                    SendMessage.CopyTo(SendBytesOffset, WriteEventArgs.Buffer, Token.BufferOffset, NumBytesToSend);
                    WriteEventArgs.SetBuffer(Token.BufferOffset, NumBytesToSend);

                    // We have to exit the lock() before we can handle the event manually
                    WaitingOnAsync = true;
                    willRaiseEvent = Connection.SendAsync(WriteEventArgs);
                }
            }
            catch (ObjectDisposedException)
            {
                WaitingOnAsync = false;
                Close();
            }

            // If we wont raise the IO event, that means a connection sent the messsage syncronously
            if (!willRaiseEvent)
            {
                // Remember, if we are here, data was sent Synchronously... IOComplete event is not called! 
                // First, Check for a closed conenction
                if (WriteEventArgs.BytesTransferred == 0 || WriteEventArgs.SocketError != SocketError.Success)
                {
                    Close();
                    return;
                }
                // Append to the offset
                SendBytesOffset += WriteEventArgs.BytesTransferred;
                WaitingOnAsync = false;
                ProcessSend();
            }
        }

        /// <summary>
        /// If there was a socket error, it can be handled propery here
        /// </summary>
        /// <param name="socketError"></param>
        private void HandleSocketError(SocketError socketError)
        {
            if (socketError != SocketError.Success)
            {
                if (!SocketClosed)
                    Close();
            }

            /* Error Handle Here
            switch (socketError)
            {
                case SocketError.TooManyOpenSockets:
                case SocketError.Shutdown:
                case SocketError.Disconnecting:
                case SocketError.ConnectionReset:
                case SocketError.NotConnected:
                case SocketError.TimedOut:  
                case SocketError.ProcessLimit:
                case SocketError.SocketError:
                case SocketError.HostDown:
                case SocketError.NetworkDown:
                case SocketError.NetworkReset:
                case SocketError.NetworkUnreachable:
                    break;
            }
            */
        }

        /// <summary>
        /// Event called when data has been recived from the client
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IOComplete(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive();
                    break;
                case SocketAsyncOperation.Send:
                    // Check for a closed conenction
                    if (e.BytesTransferred == 0 || WriteEventArgs.SocketError != SocketError.Success)
                    {
                        Close();
                        return;
                    }

                    // Append to the offset
                    SendBytesOffset += e.BytesTransferred;
                    WaitingOnAsync = false;
                    ProcessSend();
                    break;
            }
        }

        //public void ToLog(LogLevel level, string status, string statusinfo,string message,params object[] items)
        //{
        //    string temp1 = string.Format(message, items);
        //    string temp2 = string.Format("{0} [{1}] {2}: {3}",SocketManager.ServerName, status,statusinfo, temp1);
        //    LogWriter.Log.Write(LogLevel.Debug, temp2);
        
    }
}
