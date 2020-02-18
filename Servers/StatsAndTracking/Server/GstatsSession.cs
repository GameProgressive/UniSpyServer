using GameSpyLib.Common;
using GameSpyLib.Encryption;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using StatsAndTracking.Handler.CommandSwitcher;
using System.Collections.Generic;
using System.Text;

namespace StatsAndTracking
{
    public class GstatsSession : TemplateTcpSession
    {
        public uint ConnID;
        public GstatsSession(TemplateTcpServer server) : base(server)
        {
            DisconnectAfterSend = false;
        }

        protected override void OnReceived(string message)
        {
            if (message[0] != '\\')
            {
                return;
            }
            string[] recieved = message.TrimStart('\\').Split('\\');
            Dictionary<string, string> dict = GameSpyUtils.ConvertGPResponseToKeyValue(recieved);

            CommandSwitcher.Switch(this, dict);
        }


        protected override void OnConnected()
        {
            string challenge = GenerateServerChallenge();
            this.SendAsync(challenge);
            base.OnConnected();
        }
        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            if (size > 2048)
            {
                ToLog("[Spam] client spam we ignored!");
                return;
            }
            string message = Encoding.UTF8.GetString(buffer, 0, (int)size);
            message = message.Replace(@"\final\", "");
            string decodedmsg = GstatsXOR(message) + @"\final\";
            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write(LogLevel.Debug, "{0}[Recv] TCP data: {1}", ServerName, decodedmsg);
            OnReceived(decodedmsg);
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
            string sendingBuffer = Encoding.UTF8.GetString(buffer);

            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write(LogLevel.Debug, @"{0}[Send] TCP data: {1}\final\", ServerName, sendingBuffer);

            sendingBuffer = GstatsXOR(sendingBuffer) + @"\final\";

            bool returnValue = BaseSendAsync(Encoding.UTF8.GetBytes(sendingBuffer), offset, sendingBuffer.Length);

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
            string sendingBuffer = Encoding.UTF8.GetString(buffer);

            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write(LogLevel.Debug, @"{0}[Send] TCP data: {1}\final\", ServerName, sendingBuffer);

            sendingBuffer = GstatsXOR(sendingBuffer);

            long returnValue = BaseSend(Encoding.UTF8.GetBytes(sendingBuffer), offset, sendingBuffer.Length);

            if (DisconnectAfterSend)
                Disconnect();

            return returnValue;
        }

        public string GenerateServerChallenge()
        {
            //response total length bigger than 38bytes
            // challenge length should be bigger than 20bytes
            string serverChallengeKey = GameSpyRandom.GenerateRandomString(20, GameSpyRandom.StringType.Alpha);
            //string sendingBuffer = string.Format(@"\challenge\{0}\final\", ServerChallengeKey);
            //sendingBuffer = xor(sendingBuffer);
            string sendingBuffer = string.Format(@"\challenge\{0}", serverChallengeKey);
            return sendingBuffer;
        }

        /// <summary>
        /// Encrypt and Decrypt using XOR with type3 method
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string GstatsXOR(string msg)
        {
            return Enctypex.XorEncoding(msg, Enctypex.XorEncodingType.Type1);
        }
    }
}
