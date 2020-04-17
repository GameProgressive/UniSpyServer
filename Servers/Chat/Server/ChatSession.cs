using System;
using System.Text;
using Chat.Entity.Structure;
using Chat.Handler.CommandSwitcher;
using Chat.Handler.SystemHandler.Encryption;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using GameSpyLib.Network;

namespace Chat.Server
{
    public class ChatSession : TemplateTcpSession
    {
        public ChatClientInfo ClientInfo;

        public ChatSession(ChatServer server) : base(server)
        {
            ClientInfo = new ChatClientInfo();
        }

        protected override void OnReceived(string message)
        {
            base.OnReceived(message);

            new ChatCommandSwitcher().Switch(this, message);
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            if (ClientInfo.UseEncryption)
            {
                DecryptData(ref buffer, size);
            }
            LogWriter.ToLog(Serilog.Events.LogEventLevel.Debug, $"[Recv] {StringExtensions.ReplaceUnreadableCharToHex(buffer, 0, (int)size)}");
            byte[] temp = new byte[size];
            Array.Copy(buffer, temp, size);
            OnReceived(Encoding.ASCII.GetString(temp));
        }

        public override bool SendAsync(byte[] buffer, long offset, long size)
        {
            LogWriter.ToLog(Serilog.Events.LogEventLevel.Debug,
                $"[Send] {StringExtensions.ReplaceUnreadableCharToHex(buffer, 0, (int)size)}");

            if (ClientInfo.UseEncryption)
            {
                EncryptData(ref buffer, size);
            }
            return BaseSendAsync(buffer, offset, size);
        }

        private void DecryptData(ref byte[] data, long size)
        {
            ChatCrypt.Handle(ClientInfo.ClientCTX, ref data, size);
        }

        private void EncryptData(ref byte[] buffer, long size)
        {
            ChatCrypt.Handle(ClientInfo.ServerCTX, ref buffer, size);
        }
    }
}
