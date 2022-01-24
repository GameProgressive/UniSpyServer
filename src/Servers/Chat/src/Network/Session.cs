using System;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Handler.CmdHandler.General;
using UniSpyServer.Servers.Chat.Handler.CommandSwitcher;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Network.Tcp.Server;
namespace UniSpyServer.Servers.Chat.Network
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
            // user is not join any channel so we 
            // do not need to clean up anything
            if (UserInfo.JoinedChannels.Count == 0)
            {
                return;
            }
            var request = new QuitRequest()
            {
                Reason = "User disconnected"
            };
            new QuitHandler(this, request).Handle();
            base.OnDisconnected();
        }
        protected override byte[] Encrypt(byte[] buffer)
        {
            if (UserInfo.IsUsingEncryption)
            {
                return ChatCrypt.Handle(UserInfo.ServerCTX, buffer);
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
