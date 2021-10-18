using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request.General;
using Chat.Handler.CmdHandler.General;
using Chat.Handler.CommandSwitcher;
using UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;
namespace Chat.Network
{
    public sealed class Session : UniSpyTcpSession
    {
        public UserInfo UserInfo { get; private set; }
        public Session(Server server) : base(server)
        {
            UserInfo = new UserInfo(this);
        }
        protected override void OnReceived(string message) => new CmdSwitcher(this, message).Switch();
        protected override void OnDisconnected()
        {
            var request = new QuitRequest()
            {
                Reason = "Server Host leaves channel"
            };
            new QuitHandler(this, request).Handle();
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
