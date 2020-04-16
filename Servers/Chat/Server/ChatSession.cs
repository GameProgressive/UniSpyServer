using Chat.Application;
using Chat.Entity.Structure;
using Chat.Handler.CommandSwitcher;
using Chat.Handler.SystemHandler.Encryption;
using Chat.Server;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
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
            string data = Encoding.ASCII.GetString(buffer,0,(int)size);

            LogWriter.ToLog(Serilog.Events.LogEventLevel.Debug, $"[Recv] {StringExtensions.ReplaceUnreadableCharToHex(buffer, 0, (int)size)}");

          new  ChatCommandSwitcher().Switch(this, data);
        }

        private void DecryptData(ref byte[] data, long size)
        {
            ChatCrypt.Handle(UserInfo.ClientCTX, ref data, size);
        }

        public override bool SendAsync(byte[] buffer, long offset, long size)
        {
            LogWriter.ToLog(Serilog.Events.LogEventLevel.Debug,
                $"[Send] {StringExtensions.ReplaceUnreadableCharToHex(buffer,0,(int)size)}");

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
