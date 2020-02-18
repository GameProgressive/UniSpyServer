using Chat.Entity.Structure;
using Chat.Handler.CommandSwitcher;
using Chat.Handler.SystemHandler.Encryption;
using GameSpyLib.Common;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using System.Text;

namespace Chat
{
    public class ChatSession : TemplateTcpSession
    {

        public ChatUserInfo chatUserInfo { get; set; }

        public ChatSession(TemplateTcpServer server) : base(server)
        {
            chatUserInfo = new ChatUserInfo();
        }


        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            if (chatUserInfo.encrypted)
                DecryptData(ref buffer, size);

            string data = Encoding.UTF8.GetString(buffer, 0, (int)size);

            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write(LogLevel.Debug, "{0}[Recv] IRC data: {1}", ServerName, data);

            HandleIRCCommands(data);
        }

        /// <summary>
        /// Elevates security, for use with CRYPT method.
        /// </summary>
        public void ElevateSecurity(string secretKey)
        {
            string Info = $"{ServerName} Elevating security for user {Id} with game {chatUserInfo.gameName}";
            ToLog(Info);

            // 1. Generate the two keys
            string clientKey = GameSpyRandom.GenerateRandomString(16, GameSpyRandom.StringType.Alpha);
            string serverKey = GameSpyRandom.GenerateRandomString(16, GameSpyRandom.StringType.Alpha);

            // 2. Prepare two keys
            ChatCrypt.Init(chatUserInfo.ClientCTX, clientKey, secretKey);
            ChatCrypt.Init(chatUserInfo.ServerCTX, serverKey, secretKey);

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

        private void DecryptData(ref byte[] data, long size)
        {
            ChatCrypt.Handle(chatUserInfo.ClientCTX, ref data, size);
        }

        public override bool SendAsync(byte[] buffer, long offset, long size)
        {
            if (LogWriter.Log.DebugSockets)
                LogWriter.Log.Write(LogLevel.Debug, "{0}[Send] IRC data: {1}", ServerName, Encoding.UTF8.GetString(buffer));

            if (chatUserInfo.encrypted)
                EncryptData(ref buffer, size);

            return BaseSendAsync(buffer, offset, size);
        }

        private void EncryptData(ref byte[] buffer, long size)
        {
            ChatCrypt.Handle(chatUserInfo.ServerCTX, ref buffer, size);
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

