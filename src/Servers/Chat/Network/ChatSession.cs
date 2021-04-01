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

        protected override void OnReceived(string message) => new ChatCmdSwitcher(this, message).Switch();

        public override bool SendAsync(byte[] buffer, long offset, long size)
        {
            if (UserInfo.IsQuietMode)
            {
                return false;
            }
            return base.SendAsync(buffer, offset, size);
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
