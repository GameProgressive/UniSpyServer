using Chat.Handler;
using Chat.Handler.Encryption;
using Chat.Structure;
using GameSpyLib.Common;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using System.Collections.Generic;
using System.Text;

namespace Chat
{
    public class ChatSession : TemplateTcpSession
    {
        public Dictionary<string, string> _recv;

        public ChatUserInfo chatUserInfo { get; set; }

        public ChatSession(TemplateTcpServer server) : base(server)
        {
            chatUserInfo = new ChatUserInfo();
        }

        protected override void OnReceived(string data)
        {
            if (chatUserInfo.encrypted)
                DecryptData(ref data);

            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write(LogLevel.Debug, "{0}[Recv] IRC data: {1}", ServerName, data);

            HandleIRCCommands(data);
        }

        /// <summary>
        /// Elevates security, for use with CRYPT method.
        /// </summary>
        public void ElevateSecurity(string secretKey)
        {
            LogWriter.Log.Write(LogLevel.Debug, "{0}[Recv] Elevating security for user {1} with game {2}", ServerName, chatUserInfo.nickname, chatUserInfo.gameName);

            // 1. Generate the two keys
            string clientKey = GameSpyRandom.GenerateRandomString(16, GameSpyRandom.StringType.Alpha);
            string serverKey = GameSpyRandom.GenerateRandomString(16, GameSpyRandom.StringType.Alpha);

            // 2. Prepare the two keys
            ChatCrypt.Init(ref chatUserInfo.ClientCTX, Encoding.ASCII.GetBytes(clientKey), Encoding.ASCII.GetBytes(secretKey));
            ChatCrypt.Init(ref chatUserInfo.ServerCTX, Encoding.ASCII.GetBytes(serverKey), Encoding.ASCII.GetBytes(secretKey));

            // 3. Response the crypt command
            SendCommand(ChatRPL.SecureKey, "* " + clientKey + " " + serverKey);

            // 4. Start using encrypted connection
            chatUserInfo.encrypted = true;
        }

        public void SendCommand(int id, string data)
        {
            string stringToSend = ":s " + id + " " + data + "\r\n";
            SendAsync(stringToSend);
        }

        private void DecryptData(ref string data)
        {
            byte[] array = Encoding.ASCII.GetBytes(data);
            ChatCrypt.Handle(ref chatUserInfo.ClientCTX, ref array);
            data = Encoding.ASCII.GetString(array);
        }

        public override bool SendAsync(byte[] buffer, long offset, long size)
        {
            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write(LogLevel.Debug, "{0}[Send] IRC data: {1}", ServerName, Encoding.UTF8.GetString(buffer));

            if (chatUserInfo.encrypted)
                EncryptData(ref buffer, ref size);

            return base.SendAsync(buffer, offset, size);
        }

        private void EncryptData(ref byte[] buffer, ref long size)
        {
            ChatCrypt.Handle(ref chatUserInfo.ServerCTX, ref buffer);
            size = buffer.Length;
        }

        private void HandleIRCCommands(string data)
        {
            string[] messages = data.Split("\r\n");

            foreach (string message in messages)
            {
                if (message.Length < 1)
                    continue;

                string[] request = message.Trim(' ').Split(' ');
                CommandSwitcher.Switch(this, request);
            }
        }

    }
}

