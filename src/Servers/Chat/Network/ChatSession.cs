using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request.General;
using Chat.Handler.CmdHandler.General;
using Chat.Handler.CommandSwitcher;
using UniSpyLib.Network;

namespace Chat.Network
{
    internal sealed class ChatSession : UniSpyTCPSessionBase
    {
        public ChatUserInfo UserInfo { get; private set; }

        public ChatSession(ChatServer server) : base(server)
        {
            UserInfo = new ChatUserInfo(this);
        }

        protected override void OnReceived(string message) => new ChatCommandSwitcher(this, message).Switch();

        public override bool SendAsync(byte[] buffer, long offset, long size)
        {
            if (UserInfo.IsQuietMode)
            {
                return false;
            }
            return base.SendAsync(buffer, offset, size);
        }

        protected override byte[] Encrypt(byte[] buffer)
        {
            if (UserInfo.IsUsingEncryption)
            {
                return ChatCrypt.Handle(UserInfo.ClientCTX, ref buffer);
            }
            else
            {
                return buffer;
            }
        }

        protected override byte[] Decrypt(byte[] buffer)
        {
            return ChatCrypt.Handle(UserInfo.ClientCTX, ref buffer);
        }

        protected override void OnDisconnected()
        {
            var quitRequest = new QUITRequest()
            {
                Reason = "Server Host leaves channel"
            };
            new QUITHandler(this, quitRequest).Handle();
            base.OnDisconnected();
        }
    }
}
