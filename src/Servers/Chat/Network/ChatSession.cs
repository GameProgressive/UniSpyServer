using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request.General;
using Chat.Handler.CmdHandler.General;
using Chat.Handler.CommandSwitcher;
using UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;
namespace Chat.Network
{
    internal sealed class ChatSession : UniSpyTcpSession
    {
        public ChatUserInfo UserInfo { get; private set; }
        public ChatSession(ChatServer server) : base(server)
        {
            UserInfo = new ChatUserInfo(this);
        }
        protected override void OnReceived(string message) => new ChatCmdSwitcher(this, message).Switch();
        protected override void OnDisconnected()
        {
            var request = new QUITRequest()
            {
                Reason = "Server Host leaves channel"
            };
            new QUITHandler(this, request).Handle();
            base.OnDisconnected();
        }

        protected override byte[] Encrypt(byte[] buffer)
        {
            if (UserInfo.IsUsingEncryption)
            {
                return ChatCrypt.Handle(UserInfo.ClientCTX, buffer);
            }
            else
            {
                return buffer;
            }
        }

        protected override byte[] Decrypt(byte[] buffer)
        {
            if (UserInfo.IsUsingEncryption)
            {
                return ChatCrypt.Handle(UserInfo.ClientCTX, buffer);
            }
            else
            {
                return buffer;
            }
        }
    }
}
