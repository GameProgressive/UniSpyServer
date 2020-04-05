using Chat.Application;
using Chat.Entity.Structure;
using Chat.Handler.CommandSwitcher;
using Chat.Handler.SystemHandler.Encryption;
using Chat.Server;
using GameSpyLib.Extensions;
using GameSpyLib.Network;
using System.Text;

namespace Chat
{
    public class ChatSession : TemplateTcpSession
    {
        public ChatUserInfo UserInfo { get; set; }

        public ChatProxy ChatClientProxy;

        public bool IsRecievedProxyMsg = false;
        public ChatSession(TemplateTcpServer server) : base(server)
        {
            UserInfo = new ChatUserInfo();
            ChatClientProxy = new ChatProxy(this);
        }
        protected override void OnDisconnected()
        {
            ChatClientProxy.DisconnectAsync();
            base.OnDisconnected();
        }
        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            if (UserInfo.useEncryption)
            {
                DecryptData(ref buffer, size);
            }

            string data = Encoding.UTF8.GetString(buffer, 0, (int)size);

            ToLog(Serilog.Events.LogEventLevel.Debug, $"[Recv] {StringExtensions.ReplaceUnreadableCharToHex(data)}");

            CommandSwitcher.Switch(this, data);
        }

        private void DecryptData(ref byte[] data, long size)
        {
            ChatCrypt.Handle(UserInfo.ClientCTX, ref data, size);
        }

        public override bool SendAsync(byte[] buffer, long offset, long size)
        {
            ToLog(Serilog.Events.LogEventLevel.Debug,
                $"[Send] {StringExtensions.ReplaceUnreadableCharToHex(Encoding.ASCII.GetString(buffer))}");

            if (UserInfo.useEncryption)
            {
                EncryptData(ref buffer, size);
            }

            return BaseSendAsync(buffer, offset, size);
        }

        private void EncryptData(ref byte[] buffer, long size)
        {
            ChatCrypt.Handle(UserInfo.ServerCTX, ref buffer, size);
        }
    }
}
