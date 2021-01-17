using Chat.Entity.Structure.Misc;
using Chat.Handler.CommandSwitcher;
using Chat.Handler.SystemHandler.ChannelManage;
using Chat.Handler.SystemHandler.Encryption;
using Serilog.Events;
using System;
using System.Text;
using UniSpyLib.Extensions;
using UniSpyLib.Logging;
using UniSpyLib.Network;

namespace Chat.Network
{
    public class ChatSession : UniSpyTCPSessionBase
    {
        public ChatUserInfo UserInfo { get; protected set; }

        public ChatSession(ChatServer server) : base(server)
        {
            UserInfo = new ChatUserInfo();
        }

        protected override void OnReceived(string message)
        {
            base.OnReceived(message);

            new ChatCommandSwitcher(this, message).Switch();
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            if (UserInfo.UseEncryption)
            {
                DecryptData(ref buffer, size);
            }
            LogWriter.ToLog(LogEventLevel.Debug, $"[Recv] {StringExtensions.ReplaceUnreadableCharToHex(buffer, 0, (int)size)}");
            byte[] temp = new byte[size];
            Array.Copy(buffer, temp, size);
            OnReceived(Encoding.ASCII.GetString(temp));
        }


        public override bool SendAsync(byte[] buffer, long offset, long size)
        {
            LogWriter.ToLog(LogEventLevel.Debug,
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
                channel.LeaveChannel(this, "Disconnected");
            }

            ChatServer.

            base.OnDisconnected();
        }

        protected override void OnConnected()
        {
            base.OnConnected();
            UserInfo.SetDefaultUserInfo(RemoteEndPoint);
        }
    }
}
