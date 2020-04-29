using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatUser;
using Chat.Handler.CommandSwitcher;
using Chat.Handler.SystemHandler.ChannelManage;
using Chat.Handler.SystemHandler.ChatSessionManage;
using Chat.Handler.SystemHandler.Encryption;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using GameSpyLib.Network;
using System;
using System.Linq;
using System.Text;

namespace Chat.Server
{
    public class ChatSession : TemplateTcpSession
    {
        public ChatUserInfo UserInfo;

        public ChatSession(ChatServer server) : base(server)
        {
            UserInfo = new ChatUserInfo();
        }

        protected override void OnReceived(string message)
        {
            base.OnReceived(message);

            new ChatCommandSwitcher().Switch(this, message);
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            if (UserInfo.UseEncryption)
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

            if (UserInfo.UseEncryption)
            {
                EncryptData(ref buffer, size);
            }

            if (UserInfo.IsQuietMode)
            {
                return false;
            }

            return BaseSendAsync(buffer, offset, size);
        }

        private void DecryptData(ref byte[] data, long size)
        {
            ChatCrypt.Handle(UserInfo.ClientCTX, ref data, size);
        }

        private void EncryptData(ref byte[] buffer, long size)
        {
            ChatCrypt.Handle(UserInfo.ServerCTX, ref buffer, size);
        }

        protected override void OnDisconnected()
        {
            foreach (var channel in ChatChannelManager.Channels.Values)
            {
                ChatChannelUser user;

                if (!channel.GetChannelUser(this, out user))
                {
                    continue;
                }

                channel.RemoveBindOnUserAndChannel(user);
                channel.MultiCastLeave(user, "Disconnected");
            }

            ChatSessionManager.RemoveSession(this);

            base.OnDisconnected();
        }
    }
}
