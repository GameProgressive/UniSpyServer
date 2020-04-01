using Chat.Entity.Structure;
using Chat.Handler.CommandSwitcher;
using Chat.Handler.SystemHandler.Encryption;
using Chat.Server;
using GameSpyLib.Network;
using System.Text;

namespace Chat
{
    public class ChatSession : TemplateTcpSession
    {
        public ChatUserInfo chatUserInfo { get; set; }

        public ChatProxyClient ChatClientProxy;
        public ChatSession(TemplateTcpServer server) : base(server)
        {
            chatUserInfo = new ChatUserInfo();
            ChatClientProxy = new ChatProxyClient(this, "192.168.0.109", 6667);
            ChatClientProxy.ConnectAsync();
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            if (chatUserInfo.encrypted)
            {
                DecryptData(ref buffer, size);
            }

            string data = Encoding.UTF8.GetString(buffer, 0, (int)size);

            ToLog(Serilog.Events.LogEventLevel.Debug, $"[Recv] IRC data: {data}");

            CommandSwitcher.Switch(this, data);
        }

        /// <summary>
        /// Elevates security, for use with CRYPT method.
        /// </summary>
        public void ElevateSecurity(string secretKey)
        {
            string Info = $"{ServerName} Elevating security for user {Id} with game {chatUserInfo.gameName}";
            ToLog(Serilog.Events.LogEventLevel.Information, Info);

            // 1. Generate the two keys
            string clientKey = "0000000000000000";
            //GameSpyRandom.GenerateRandomString(16, GameSpyRandom.StringType.Alpha);
            string serverKey = "0000000000000000";
            //GameSpyRandom.GenerateRandomString(16, GameSpyRandom.StringType.Alpha);

            // 2. Prepare two keys
            ChatCrypt.Init(chatUserInfo.ClientCTX, clientKey, secretKey);
            ChatCrypt.Init(chatUserInfo.ServerCTX, serverKey, secretKey);

            // 3. Response the crypt command
            SendCommand(ChatRPL.SecureKey, "* " + clientKey + " " + serverKey);
            // string buffer = $":s {ChatRPL.SecureKey} * {clientKey} {serverKey}";
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

            ToLog(Serilog.Events.LogEventLevel.Debug, $"[Send] IRC data: {Encoding.ASCII.GetString(buffer)}");


            if (chatUserInfo.encrypted)
            {
                EncryptData(ref buffer, size);
            }

            return BaseSendAsync(buffer, offset, size);
        }

        private void EncryptData(ref byte[] buffer, long size)
        {
            ChatCrypt.Handle(chatUserInfo.ServerCTX, ref buffer, size);
        }
    }
}
